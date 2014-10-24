using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Mindscape.LightSpeed;
using Mindscape.LightSpeed.Caching;
using Mindscape.LightSpeed.Linq;
using Mindscape.LightSpeed.Logging;
using Mindscape.LightSpeed.Querying;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database.Schema;
using Roadkill.Core.Logging;
using Roadkill.Core.Plugins;
using Roadkill.Core.Mvc.ViewModels;
using StructureMap;
using PluginSettings = Roadkill.Core.Plugins.Settings;

namespace Roadkill.Core.Database.LightSpeed
{
    public class LightSpeedRepository : Roadkill.Core.Database.IRepository
    {
        private ApplicationSettings _applicationSettings;

        internal IQueryable<PageEntity> Pages
        {
            get
            {
                return UnitOfWork.Query<PageEntity>();
            }
        }

        internal IQueryable<PageContentEntity> PageContents
        {
            get
            {
                return UnitOfWork.Query<PageContentEntity>();
            }
        }

        internal IQueryable<UserEntity> Users
        {
            get
            {
                return UnitOfWork.Query<UserEntity>();
            }
        }

        internal IQueryable<OrgEntity> Orgs
        {
            get
            {
                return UnitOfWork.Query<OrgEntity>();
            }
        }

        internal IQueryable<RelEntity> Rels
        {
            get
            {
                return UnitOfWork.Query<RelEntity>();
            }
        }

        internal IQueryable<RelTypeEntity> RelTypes
        {
            get
            {
                return UnitOfWork.Query<RelTypeEntity>();
            }
        }

        internal IQueryable<ActivityEntity> Activites
        {
            get
            {
                return UnitOfWork.Query<ActivityEntity>();
            }
        }

        public virtual LightSpeedContext Context
        {
            get
            {
                LightSpeedContext context = ObjectFactory.GetInstance<LightSpeedContext>();
                if (context == null)
                    throw new DatabaseException("The context for Lightspeed is null - has Startup() been called?", null);

                return context;
            }
        }

        public virtual IUnitOfWork UnitOfWork
        {
            get
            {
                EnsureConectionString();

                IUnitOfWork unitOfWork = ObjectFactory.GetInstance<IUnitOfWork>();
                if (unitOfWork == null)
                    throw new DatabaseException("The IUnitOfWork for Lightspeed is null - has Startup() been called?", null);

                return unitOfWork;
            }
        }

        public LightSpeedRepository(ApplicationSettings settings)
        {
            _applicationSettings = settings;
        }

        #region IRepository
        public void Startup(DataStoreType dataStoreType, string connectionString, bool enableCache)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                LightSpeedContext context = new LightSpeedContext();
                context.ConnectionString = connectionString;
                context.DataProvider = dataStoreType.LightSpeedDbType;
                context.IdentityMethod = IdentityMethod.GuidComb;
                context.CascadeDeletes = true;
                context.VerboseLogging = true;
                context.Logger = new DatabaseLogger();

                if (enableCache)
                    context.Cache = new CacheBroker(new DefaultCache());

                ObjectFactory.Configure(x =>
                {
                    x.For<LightSpeedContext>().Singleton().Use(context);
                    x.For<IUnitOfWork>().HybridHttpOrThreadLocalScoped().Use(ctx => ctx.GetInstance<LightSpeedContext>().CreateUnitOfWork());
                });
            }
            else
            {
                Log.Warn("LightSpeedRepository.Startup skipped as no connection string was provided");
            }
        }

        public void TestConnection(DataStoreType dataStoreType, string connectionString)
        {
            LightSpeedContext context = ObjectFactory.GetInstance<LightSpeedContext>();
            if (context == null)
                throw new InvalidOperationException("Repository.Test failed - LightSpeedContext was null from the ObjectFactory");

            using (IDbConnection connection = context.DataProviderObjectFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
            }
        }
        #endregion

        #region ISettingsRepository
        public void Install(DataStoreType dataStoreType, string connectionString, bool enableCache)
        {
            LightSpeedContext context = ObjectFactory.GetInstance<LightSpeedContext>();
            if (context == null)
                throw new InvalidOperationException("Repository.Install failed - LightSpeedContext was null from the ObjectFactory");

            using (IDbConnection connection = context.DataProviderObjectFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                IDbCommand command = context.DataProviderObjectFactory.CreateCommand();
                command.Connection = connection;

                dataStoreType.Schema.Drop(command);
                dataStoreType.Schema.Create(command);
            }
        }

        public void Upgrade(ApplicationSettings settings)
        {
            try
            {
                using (IDbConnection connection = Context.DataProviderObjectFactory.CreateConnection())
                {
                    connection.ConnectionString = settings.ConnectionString;
                    connection.Open();

                    IDbCommand command = Context.DataProviderObjectFactory.CreateCommand();
                    command.Connection = connection;

                    settings.DataStoreType.Schema.Upgrade(command);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Upgrade failed: {0}", ex);
                throw new UpgradeException("A problem occurred upgrading the database schema.\n\n", ex);
            }

            try
            {
                SaveSiteSettings(new SiteSettings());
            }
            catch (Exception ex)
            {
                Log.Error("Upgrade failed: {0}", ex);
                throw new UpgradeException("A problem occurred saving the site preferences.\n\n", ex);
            }
        }

        public SiteSettings GetSiteSettings()
        {
            SiteSettings siteSettings = new SiteSettings();
            SiteConfigurationEntity entity = UnitOfWork.Find<SiteConfigurationEntity>()
                                                .FirstOrDefault(x => x.Id == SiteSettings.SiteSettingsId);

            if (entity != null)
            {
                siteSettings = SiteSettings.LoadFromJson(entity.Content);
            }
            else
            {
                Log.Warn("No site settings could be found in the database, using a default instance");
            }

            return siteSettings;
        }

        public PluginSettings GetTextPluginSettings(Guid databaseId)
        {
            PluginSettings pluginSettings = null;
            SiteConfigurationEntity entity = UnitOfWork.Find<SiteConfigurationEntity>()
                                                .FirstOrDefault(x => x.Id == databaseId);

            if (entity != null)
            {
                pluginSettings = PluginSettings.LoadFromJson(entity.Content);
            }

            return pluginSettings;
        }

        public void SaveSiteSettings(SiteSettings siteSettings)
        {
            SiteConfigurationEntity entity = UnitOfWork.Find<SiteConfigurationEntity>()
                                                .FirstOrDefault(x => x.Id == SiteSettings.SiteSettingsId);

            if (entity == null || entity.Id == Guid.Empty)
            {
                entity = new SiteConfigurationEntity();
                entity.Id = SiteSettings.SiteSettingsId;
                entity.Version = ApplicationSettings.ProductVersion.ToString();
                entity.Content = siteSettings.GetJson();
                UnitOfWork.Add(entity);
            }
            else
            {
                entity.Version = ApplicationSettings.ProductVersion.ToString();
                entity.Content = siteSettings.GetJson();
            }

            UnitOfWork.SaveChanges();
        }

        public void SaveTextPluginSettings(TextPlugin plugin)
        {
            string version = plugin.Version;
            if (string.IsNullOrEmpty(version))
                version = "1.0.0";

            SiteConfigurationEntity entity = UnitOfWork.Find<SiteConfigurationEntity>()
                                                .FirstOrDefault(x => x.Id == plugin.DatabaseId);

            if (entity == null || entity.Id == Guid.Empty)
            {
                entity = new SiteConfigurationEntity();
                entity.Id = plugin.DatabaseId;
                entity.Version = version;
                entity.Content = plugin.Settings.GetJson();
                UnitOfWork.Add(entity);
            }
            else
            {
                entity.Version = version;
                entity.Content = plugin.Settings.GetJson();
            }

            UnitOfWork.SaveChanges();
        }

        #endregion

        #region IPageRepository
        public PageContent AddNewPage(Page page, string text, string editedBy, DateTime editedOn, DateTime projectStart, DateTime projectEnd, bool projectEstimatedTime, string projectStatus, string projectLanguage, int orgID)
        {
            PageEntity pageEntity = new PageEntity();
            ToEntity.FromPage(page, pageEntity);
            pageEntity.Id = 0;
            UnitOfWork.Add(pageEntity);
            UnitOfWork.SaveChanges();

            PageContentEntity pageContentEntity = new PageContentEntity()
            {
                Id = Guid.NewGuid(),
                Page = pageEntity,
                Text = text,
                EditedBy = editedBy,
                EditedOn = editedOn,
                VersionNumber = 1,
                ProjectStart = projectStart,
                ProjectEnd = projectEnd,
                ProjectEstimatedTime = projectEstimatedTime,
                ProjectStatus = projectStatus,
                ProjectLanguage = projectLanguage,
                orgID = orgID,
            };

            UnitOfWork.Add(pageContentEntity);
            UnitOfWork.SaveChanges();

            PageContent pageContent = FromEntity.ToPageContent(pageContentEntity);
            pageContent.Page = FromEntity.ToPage(pageEntity);
            return pageContent;
        }

        public PageContent AddNewPageContentVersion(Page page, string text, string editedBy, DateTime editedOn, int version, DateTime projectStart, DateTime projectEnd, bool projectEstimatedTime, string projectStatus, string projectLanguage, int orgID)
        {
            if (version < 1)
                version = 1;

            PageEntity pageEntity = UnitOfWork.FindById<PageEntity>(page.Id);
            if (pageEntity != null)
            {
                // Update the content
                PageContentEntity pageContentEntity = new PageContentEntity()
                {
                    Id = Guid.NewGuid(),
                    Page = pageEntity,
                    Text = text,
                    EditedBy = editedBy,
                    EditedOn = editedOn,
                    VersionNumber = version,
                    ProjectStart = projectStart,
                    ProjectEnd = projectEnd,
                    ProjectEstimatedTime = projectEstimatedTime,
                    ProjectStatus = projectStatus,
                    ProjectLanguage = projectLanguage,
                    orgID = orgID,
                };

                UnitOfWork.Add(pageContentEntity);
                UnitOfWork.SaveChanges();

                // The page modified fields
                pageEntity.ModifiedOn = editedOn;
                pageEntity.ModifiedBy = editedBy;
                UnitOfWork.SaveChanges();

                // Turn the content database entity back into a domain object
                PageContent pageContent = FromEntity.ToPageContent(pageContentEntity);
                pageContent.Page = FromEntity.ToPage(pageEntity);

                return pageContent;
            }

            Log.Error("Unable to update page content for page id {0} (not found)", page.Id);
            return null;
        }

        public IEnumerable<Page> AllPages()
        {
            List<PageEntity> entities = Pages.ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<PageContent> AllPageContents()
        {
            List<PageContentEntity> entities = PageContents.ToList();
            return FromEntity.ToPageContentList(entities);
        }

        public IEnumerable<string> AllTags()
        {
            return new List<string>(Pages.Select(p => p.Tags));
        }

        public void DeleteAllPages()
        {
            UnitOfWork.Remove(new Query(typeof(PageEntity)));
            UnitOfWork.SaveChanges();

            UnitOfWork.Remove(new Query(typeof(PageContentEntity)));
            UnitOfWork.SaveChanges();
        }

        public void DeletePage(Page page)
        {
            PageEntity entity = UnitOfWork.FindById<PageEntity>(page.Id);
            UnitOfWork.Remove(entity);
            UnitOfWork.SaveChanges();
        }

        public void DeletePageContent(PageContent pageContent)
        {
            PageContentEntity entity = UnitOfWork.FindById<PageContentEntity>(pageContent.Id);
            UnitOfWork.Remove(entity);
            UnitOfWork.SaveChanges();
        }

        public IEnumerable<Page> FindPagesCreatedBy(string username)
        {
            List<PageEntity> entities = Pages.Where(p => p.CreatedBy == username).ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindPagesModifiedBy(string username)
        {
            List<PageEntity> entities = Pages.Where(p => p.ModifiedBy == username).ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindPagesContainingTag(string tag)
        {
            IEnumerable<PageEntity> entities = Pages.Where(p => p.Tags.ToLower().Contains(tag.ToLower())); // Lightspeed doesn't support ToLowerInvariant
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<PageContent> FindPageContentsByPageId(int pageId)
        {
            List<PageContentEntity> entities = PageContents.Where(p => p.Page.Id == pageId).ToList();
            return FromEntity.ToPageContentList(entities);
        }

        public IEnumerable<PageContent> FindPageContentsEditedBy(string username)
        {
            List<PageContentEntity> entities = PageContents.Where(p => p.EditedBy == username).ToList();
            return FromEntity.ToPageContentList(entities);
        }

        public Page GetPageById(int id)
        {
            PageEntity entity = Pages.FirstOrDefault(p => p.Id == id);
            return FromEntity.ToPage(entity);
        }

        public Page GetPageByTitle(string title)
        {
            PageEntity entity = Pages.FirstOrDefault(p => p.Title.ToLower() == title.ToLower());
            return FromEntity.ToPage(entity);
        }

        public PageContent GetLatestPageContent(int pageId)
        {
            PageContentEntity entity = PageContents.Where(x => x.Page.Id == pageId).OrderByDescending(x => x.EditedOn).FirstOrDefault();
            return FromEntity.ToPageContent(entity);
        }

        public PageContent GetPageContentById(Guid id)
        {
            PageContentEntity entity = PageContents.FirstOrDefault(p => p.Id == id);
            return FromEntity.ToPageContent(entity);
        }

        public PageContent GetPageContentByPageIdAndVersionNumber(int id, int versionNumber)
        {
            PageContentEntity entity = PageContents.FirstOrDefault(p => p.Page.Id == id && p.VersionNumber == versionNumber);
            return FromEntity.ToPageContent(entity);
        }

        public IEnumerable<PageContent> GetPageContentByEditedBy(string username)
        {
            List<PageContentEntity> entities = PageContents.Where(p => p.EditedBy == username).ToList();
            return FromEntity.ToPageContentList(entities);
        }

        public Page SaveOrUpdatePage(Page page)
        {
            PageEntity entity = UnitOfWork.FindById<PageEntity>(page.Id);
            if (entity == null)
            {
                entity = new PageEntity();
                ToEntity.FromPage(page, entity);
                UnitOfWork.Add(entity);
                UnitOfWork.SaveChanges();
                page = FromEntity.ToPage(entity);
            }
            else
            {
                ToEntity.FromPage(page, entity);
                UnitOfWork.SaveChanges();
                page = FromEntity.ToPage(entity);
            }

            return page;
        }

        /// <summary>
        /// This updates an existing set of text and is used for page rename updates.
        /// To add a new version of a page, use AddNewPageContentVersion
        /// </summary>
        /// <param name="content"></param>
        public void UpdatePageContent(PageContent content)
        {
            PageContentEntity entity = UnitOfWork.FindById<PageContentEntity>(content.Id);
            if (entity != null)
            {
                ToEntity.FromPageContent(content, entity);
                UnitOfWork.SaveChanges();
                content = FromEntity.ToPageContent(entity);
            }
        }


        public List<Relationship> FindPageRelationships(int pageID)
        {
            List<RelEntity> entities = Rels.Where(p => p.pageId == pageID).ToList();
            return FromEntity.ToRelList(entities);
        }




        #endregion

        #region IUserRepository
        public IEnumerable<User> AllUsers()
        {
            List<UserEntity> entities = Users.ToList();
            return FromEntity.ToUserList(entities);
        }

        public void DeleteUser(User user)
        {
            UserEntity entity = UnitOfWork.FindById<UserEntity>(user.Id);
            UnitOfWork.Remove(entity);
            UnitOfWork.SaveChanges();
        }

        public void DeleteAllUsers()
        {
            UnitOfWork.Remove(new Query(typeof(UserEntity)));
            UnitOfWork.SaveChanges();
        }

        public User GetAdminById(Guid id)
        {
            UserEntity entity = Users.FirstOrDefault(x => x.Id == id && x.IsAdmin);
            return FromEntity.ToUser(entity);
        }

        public User GetUserByActivationKey(string key)
        {
            UserEntity entity = Users.FirstOrDefault(x => x.ActivationKey == key && x.IsActivated == false);
            return FromEntity.ToUser(entity);
        }

        public User GetEditorById(Guid id)
        {
            UserEntity entity = Users.FirstOrDefault(x => x.Id == id && x.IsEditor);
            return FromEntity.ToUser(entity);
        }

        public User GetUserByEmail(string email, bool? isActivated = null)
        {
            UserEntity entity;

            if (isActivated.HasValue)
                entity = Users.FirstOrDefault(x => x.Email == email && x.IsActivated == isActivated);
            else
                entity = Users.FirstOrDefault(x => x.Email == email);

            return FromEntity.ToUser(entity);
        }

        public User GetUserById(Guid id, bool? isActivated = null)
        {
            UserEntity entity;

            if (isActivated.HasValue)
                entity = Users.FirstOrDefault(x => x.Id == id && x.IsActivated == isActivated);
            else
                entity = Users.FirstOrDefault(x => x.Id == id);

            return FromEntity.ToUser(entity);
        }

        public User GetUserByPasswordResetKey(string key)
        {
            UserEntity entity = Users.FirstOrDefault(x => x.PasswordResetKey == key);
            return FromEntity.ToUser(entity);
        }

        public User GetUserByUsername(string username)
        {
            UserEntity entity = Users.FirstOrDefault(x => x.Username == username);
            return FromEntity.ToUser(entity);
        }

        public User GetUserByUsernameOrEmail(string username, string email)
        {
            UserEntity entity = Users.FirstOrDefault(x => x.Username == username || x.Email == email);
            return FromEntity.ToUser(entity);
        }

        public IEnumerable<User> FindAllEditors()
        {
            List<UserEntity> entities = Users.Where(x => x.IsEditor).ToList();
            return FromEntity.ToUserList(entities);
        }

        public IEnumerable<User> FindAllAdmins()
        {
            List<UserEntity> entities = Users.Where(x => x.IsAdmin).ToList();
            return FromEntity.ToUserList(entities);
        }

        public User SaveOrUpdateUser(User user)
        {
            UserEntity entity = UnitOfWork.FindById<UserEntity>(user.Id);
            if (entity == null)
            {
                // Turn the domain object into a database entity
                entity = new UserEntity();
                ToEntity.FromUser(user, entity);
                UnitOfWork.Add(entity);
                UnitOfWork.SaveChanges();

                user = FromEntity.ToUser(entity);
            }
            else
            {
                ToEntity.FromUser(user, entity);
                UnitOfWork.SaveChanges();
            }

            return user;
        }

        #endregion


        #region IOrgRepository

        public Organisation GetOrgByID(int id)
        {
            OrgEntity entity = Orgs.FirstOrDefault(x => x.Id == id);
            return FromEntity.ToOrg(entity);
        }

        public IEnumerable<Organisation> FindAllOrgs()
        {
            List<OrgEntity> entities = Orgs.ToList();
            return FromEntity.ToOrgList(entities);
        }

        #endregion


        #region IRelRepository

        public RelationshipType GetRelTypeByID(int id)
        {
            RelTypeEntity entity = RelTypes.FirstOrDefault(x => x.Id == id);
            return FromEntity.ToRelType(entity);
        }

        public Relationship AddNewRel(Relationship rel, int reltypeid, string username, int orgID, int pageID, string reltext)
        {

            RelEntity relEntity = new RelEntity();
            ToEntity.FromRelationship(rel, relEntity);
            relEntity.id = 0;
            relEntity.username = username;

            User _user = GetUserByUsername(username);
            relEntity.orgID = _user.orgID;

            relEntity.pageId = pageID;
            relEntity.relDateTime = DateTime.Now;
            UnitOfWork.Add(relEntity);
            UnitOfWork.SaveChanges();

            Relationship newrel = FromEntity.ToRel(relEntity);
            return newrel;
        }


        public Relationship SaveOrUpdateRel(Relationship rel)
        {
            RelEntity entity = UnitOfWork.FindById<RelEntity>(rel.id);
            if (entity == null)
            {

                entity = new RelEntity();
                ToEntity.FromRelationship(rel, entity);
                UnitOfWork.Add(entity);
                UnitOfWork.SaveChanges();
                rel = FromEntity.ToRel(entity);
            }
            else
            {
                rel.orgID = GetUserByUsername(rel.username).orgID;

                ToEntity.FromRelationship(rel, entity);
                UnitOfWork.SaveChanges();
                rel = FromEntity.ToRel(entity);
            }

            return rel;
        }

        public Relationship GetRelById(int id)
        {
            RelEntity entity = Rels.FirstOrDefault(p => p.id == id);
            return FromEntity.ToRel(entity);
        }

        public IEnumerable<Relationship> GetRelByPage(int pageid)
        {
            List<RelEntity> entities = Rels.Where(p => p.pageId == pageid).ToList();
            return FromEntity.ToRelList(entities);
        }

        public IEnumerable<Relationship> FindAllRels()
        {
            List<RelEntity> entities = Rels.ToList();
            return FromEntity.ToRelList(entities);
        }

        public void DeleteRel(Relationship rel)
        {
            RelEntity entity = UnitOfWork.FindById<RelEntity>(rel.id);
            UnitOfWork.Remove(entity);
            UnitOfWork.SaveChanges();
        }

        public IEnumerable<Relationship> FindRelsCreatedBy(string username)
        {
            List<RelEntity> entities = Rels.Where(p => p.username == username).ToList();
            return FromEntity.ToRelList(entities);
        }

        public IEnumerable<RelationshipType> FindAllRelTypes()
        {
            List<RelTypeEntity> entities = RelTypes.ToList();
            return FromEntity.ToRelTypeList(entities);
        }

        public Relationship RelToUserToPage(int pageID, string username)
        {
            Relationship _RelToUserToPage = new Relationship();

            RelEntity entity = Rels.FirstOrDefault(r => r.username == username && r.pageId == pageID);

            if (entity != null && entity.pageId != 0)
            {
                _RelToUserToPage = FromEntity.ToRel(entity);

            }

            return _RelToUserToPage;
        }

        public Organisation GetOrgByUser(string username)
        {
            UserEntity userentity = Users.FirstOrDefault(p => p.Username == username);
            OrgEntity orgentity = Orgs.FirstOrDefault(p => p.Id == userentity.orgID);
            return FromEntity.ToOrg(orgentity);
        }

        /// <summary>
        /// Gets an IEnumerable{SelectListItem} of project statuses, as a default
        /// SelectList doesn't add option value attributes.
        /// </summary>
        public IEnumerable<Activity> ActivityViewList()
        {


            IEnumerable<Relationship> RelList;
            RelList = FindAllRels();
            RelList.OrderByDescending(x => x.relDateTime);
            RelList = RelList.Take(10).ToList();

            List<Activity> items = new List<Activity>();



            foreach (Relationship rel in RelList)
            {
                bool fail = false;

                Activity item = new Activity();

                try
                {
                    User user = new User();
                    user = GetUserByUsername(rel.username);
                    item.userNames = user.Firstname + " " + user.Lastname;
                }
                catch { fail = true; }

                try
                {
                    RelationshipType reltype = new RelationshipType();
                    reltype = GetRelTypeByID(rel.relTypeId);

                    string activityPastTense = "";
                    if (reltype.relTypeText == "Like")
                    {
                        activityPastTense = "liked";
                    }
                    if (reltype.relTypeText == "Join")
                    {
                        activityPastTense = "joined";
                    }

                    item.activityName = activityPastTense;
                }
                catch { fail = true; }

                try
                {
                    item.activityDateTime = rel.relDateTime;
                }
                catch { fail = true; }

                try
                {

                    //get the page the relationship is related to
                    Page page = new Page();
                    page = GetPageById(rel.pageId);
                    item.projectName = page.Title;
                    item.projectId = page.Id;

                    //get the orgainsation that owns the page
                    Organisation org = new Organisation();
                    org = GetOrgByID(page.orgID);
                    item.orgName = org.OrgName;
                }
                catch { fail = true; }

                if (fail == false)
                {
                    items.Add(item);
                }
            }



            IEnumerable<Page> PageList;
            PageList = AllPages();
            PageList.OrderByDescending(x => x.ModifiedOn);
            PageList = PageList.Take(10).ToList();


            foreach (Page page in PageList)
            {

                Activity item = new Activity();

                try
                {
                    User user = new User();
                    user = GetUserByUsername(page.ModifiedBy);
                    item.userNames = user.Firstname + " " + user.Lastname;
                }
                catch { }

                try
                {
                    string activityPastTense = "";

                    if (page.ModifiedOn == page.CreatedOn)
                    {
                        activityPastTense = "added";
                    }
                    else
                    {
                        activityPastTense = "edited";
                    }

                    item.activityName = activityPastTense;
                }
                catch { }

                try
                {
                    item.activityDateTime = page.ModifiedOn;
                }
                catch { }

                try
                {

                    item.projectName = page.Title;
                    item.projectId = page.Id;

                    //get the orgainsation that owns the page
                    Organisation org = new Organisation();
                    org = GetOrgByID(page.orgID);
                    item.orgName = org.OrgName;
                }
                catch { }

                items.Add(item);
            }

            DateTime filter = new DateTime(2014, 10, 1);

            IEnumerable<User> UserList;
            UserList = AllUsers();
            UserList.OrderByDescending(x => x.createdOn);
            UserList = UserList.Where(x => x.createdOn.Date > filter).ToList();
            UserList = UserList.Take(10).ToList();


            foreach (User user in UserList)
            {

                Activity item = new Activity();

                item.userNames = user.Firstname + " " + user.Lastname;
                item.activityName = "signup";
                item.activityDateTime = user.createdOn;

                item.projectName = "";
                item.projectId = 0;

                //get the orgainsation that owns the page
                Organisation org = new Organisation();
                org = GetOrgByID(user.orgID);

                string orgname = org.OrgName;

                if (orgname.Contains("_") != true)
                {
                    item.orgName = " from " + orgname;
                }

                items.Add(item);
            } 

            items.OrderByDescending(x => x.activityDateTime);
            items = items.Take(15).ToList();

            return items;

        }




        #endregion

        #region IDisposable
        public void Dispose()
        {
            UnitOfWork.SaveChanges();
            UnitOfWork.Dispose();
        }
        #endregion

        private void EnsureConectionString()
        {
            if (_applicationSettings.Installed && string.IsNullOrEmpty(_applicationSettings.ConnectionString))
                throw new DatabaseException("The connection string is empty in the web.config file (and the roadkill.config's installed=true).", null);
        }
    }
}
