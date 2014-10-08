using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Roadkill.Core.Converters;
using Roadkill.Core.Database;
using Roadkill.Core.Cache;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Configuration;
using System.Web;
using Roadkill.Core.Logging;
using Roadkill.Core.Text;
using Roadkill.Core.Plugins;

namespace Roadkill.Core.Services
{
    /// <summary>
    /// Provides a set of tasks for wiki page management.
    /// </summary>
    public class RelService : ServiceBase, IRelService
    {
        
        private SearchService _searchService;
        private MarkupConverter _markupConverter;
        private PageHistoryService _historyService;
        private IUserContext _context;
        private ListCache _listCache;
        private PageViewModelCache _pageViewModelCache;
        private SiteCache _siteCache;
        private IPluginFactory _pluginFactory;

        public RelService(ApplicationSettings settings, IRepository repository, SearchService searchService,
            PageHistoryService historyService, IUserContext context,
            ListCache listCache, PageViewModelCache pageViewModelCache, SiteCache sitecache, IPluginFactory pluginFactory)
            : base(settings, repository)
        {
            _searchService = searchService;
            _markupConverter = new MarkupConverter(settings, repository, pluginFactory);
            _historyService = historyService;
            _context = context;
            _listCache = listCache;
            _pageViewModelCache = pageViewModelCache;
            _siteCache = sitecache;
            _pluginFactory = pluginFactory;
        }

        /// <summary>
        /// Adds the page to the database.
        /// </summary>
        /// <param name="model">The summary details for the page.</param>
        /// <returns>A <see cref="PageViewModel"/> for the newly added page.</returns>
        /// <exception cref="DatabaseException">An databaseerror occurred while saving.</exception>
        /// <exception cref="SearchException">An error occurred adding the page to the search index.</exception>
        public RelViewModel AddRel(RelViewModel model)
        {
            try
            {
                string currentUser = _context.CurrentUsername;

                Relationship rel = new Relationship();
                rel.id = model.id;
                rel.username = currentUser;
                rel.orgId = Repository.GetOrgByUser(currentUser).Id;
                rel.pageId = model.pageID;
                rel.relTypeId = model.relTypeID;
                rel.relText = model.reltext;

                Relationship newrel = Repository.AddNewRel(rel, rel.relTypeId, currentUser, rel.orgId, rel.pageId, rel.relText);

                return model;
            }
            catch (DatabaseException e)
            {
                throw new DatabaseException(e, "An error occurred while adding relationship '{0}' to the database", model.id);
            }
        }

        /// <summary>
        /// Retrieves a list of all pages in the system.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PageViewModel}"/> of the pages.</returns>
        /// <exception cref="DatabaseException">An databaseerror occurred while retrieving the list.</exception>
        public IEnumerable<RelViewModel> AllRels(bool loadPageContent = false)
        {
            try
            {
                string cacheKey = "";
                IEnumerable<RelViewModel> relModels;

                IEnumerable<Relationship> rels = Repository.AllRels().OrderBy(p => p.id);
                relModels = from rel in rels
                            select new RelViewModel() { id = rel.id, };

                return relModels;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred while retrieving all relationships from the database");
            }
        }

        /// <summary>
        /// Gets alls the pages created by a user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>All pages created by the provided user, or an empty list if none are found.</returns>
        /// <exception cref="DatabaseException">An databaseerror occurred while retrieving the list.</exception>
        public IEnumerable<RelViewModel> AllRelsCreatedBy(string userName)
        {
            try
            {
                string cacheKey = string.Format("allrels.createdby.{0}", userName);

                IEnumerable<RelViewModel> models = _listCache.Get<RelViewModel>(cacheKey);
                if (models == null)
                {
                    IEnumerable<Relationship> rels = Repository.FindRelsCreatedBy(userName);
                    models = from rel in rels
                             select new RelViewModel(Repository.GetRelById(rel.id));
                }

                return models;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred while retrieving all relationships created by {0} from the database", userName);
            }
        }

        

        /// <summary>
        /// Deletes a page from the database.
        /// </summary>
        /// <param name="pageId">The id of the page to remove.</param>
        /// <exception cref="DatabaseException">An databaseerror occurred while deleting the page.</exception>
        public void DeleteRel(int id)
        {
            try
            {
                // Avoid grabbing all the pagecontents coming back each time a page is requested, it has no inverse relationship.
                Relationship rel = Repository.GetRelById(id);
                Repository.DeleteRel(rel);

            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred while deleting the relationship id {0} from the database", id);
            }
        }

        /// <summary>
        /// Finds all pages with the given tag.
        /// </summary>
        /// <param name="tag">The tag to search for.</param>
        /// <returns>A <see cref="IEnumerable{PageViewModel}"/> of pages tagged with the provided tag.</returns>
        /// <exception cref="DatabaseException">An database error occurred while getting the list.</exception>
        public IEnumerable<PageViewModel> FindByTag(string tag)
        {
            try
            {
                string cacheKey = string.Format("pagesbytag.{0}", tag);

                IEnumerable<PageViewModel> models = _listCache.Get<PageViewModel>(cacheKey);
                if (models == null)
                {

                    IEnumerable<Page> pages = Repository.FindPagesContainingTag(tag).OrderBy(p => p.Title);
                    models = from page in pages
                             select new PageViewModel(Repository.GetLatestPageContent(page.Id), _markupConverter);

                    _listCache.Add<PageViewModel>(cacheKey, models);
                }

                return models;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred finding the tag '{0}' in the database", tag);
            }
        }

        /// <summary>
        /// Finds a page by its title
        /// </summary>
        /// <param name="title">The page title</param>
        /// <returns>A <see cref="PageViewModel"/> for the page.</returns>
        /// <exception cref="DatabaseException">An databaseerror occurred while getting the page.</exception>
        public PageViewModel FindByTitle(string title)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                    return null;

                Page page = Repository.GetPageByTitle(title);

                if (page == null)
                    return null;
                else
                    return new PageViewModel(Repository.GetLatestPageContent(page.Id), _markupConverter);
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred finding the page with title '{0}' in the database", title);
            }
        }

        /// <summary>
        /// Retrieves the page by its id.
        /// </summary>
        /// <param name="id">The id of the page</param>
        /// <returns>A <see cref="PageViewModel"/> for the page.</returns>
        /// <exception cref="DatabaseException">An databaseerror occurred while getting the page.</exception>
        public RelViewModel GetRelById(int id, bool loadContent = false)
        {
            try
            {
                RelViewModel relModel = new RelViewModel();

                Relationship rel = Repository.GetRelById(id);

                relModel = new RelViewModel(rel);

                return relModel;

            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred getting the page with id '{0}' from the database", id);
            }
        }

        /// <summary>
        /// Updates the provided relationship.
        /// </summary>
        /// <param name="model">The summary.</param>
        /// <exception cref="DatabaseException">An databaseerror occurred while updating.</exception>
        /// <exception cref="SearchException">An error occurred adding the page to the search index.</exception>
        public void UpdateRel(RelViewModel model)
        {
            try
            {
                string currentUser = _context.CurrentUsername;

                Relationship rel = Repository.GetRelById(model.id);
                rel.id = model.id;
                rel.username = currentUser;
                rel.orgId = model.orgID;
                rel.pageId = model.pageID;
                rel.relTypeId = model.relTypeID;
                rel.relText = model.reltext;
                rel.relDateTime = DateTime.Now;

                Repository.SaveOrUpdateRel(rel);

            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred updating the relationship with id '{0}' in the database", model.id);
            }
        }        

    }
}
