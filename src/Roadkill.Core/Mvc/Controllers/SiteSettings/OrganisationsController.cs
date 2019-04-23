using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using Ionic.Zip;
using Roadkill.Core.Localization;
using Roadkill.Core.Configuration;
using Roadkill.Core.Cache;
using Roadkill.Core.Services;
using Roadkill.Core.Import;
using Roadkill.Core.Security;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Logging;
using Roadkill.Core.Database.Export;
using Roadkill.Core.Database;
using Roadkill.Core.Plugins;
using Roadkill.Core.Domain.Export;

namespace Roadkill.Core.Mvc.Controllers
{
    /// <summary>
    /// Provides functionality for the tools page for admins.
    /// </summary>
    /// <remarks>All actions in this controller require admin rights.</remarks>
    [AdminRequired]
    public class OrganisationsController : ControllerBase
    {
        private SettingsService _settingsService;
        private PageService _pageService;
        private SearchService _searchService;
        private IWikiImporter _wikiImporter;
        private ListCache _listCache;
        private PageViewModelCache _pageViewModelCache;
        private IRepository _repository;
        private IPluginFactory _pluginFactory;
        private WikiExporter _wikiExporter;

        public OrganisationsController(ApplicationSettings settings, UserServiceBase userManager,
            SettingsService settingsService, PageService pageService, SearchService searchService, IUserContext context,
            ListCache listCache, PageViewModelCache pageViewModelCache, IWikiImporter wikiImporter,
            IRepository repository, IPluginFactory pluginFactory, WikiExporter wikiExporter)
            : base(settings, userManager, context, settingsService)
        {
            _settingsService = settingsService;
            _pageService = pageService;
            _searchService = searchService;
            _listCache = listCache;
            _pageViewModelCache = pageViewModelCache;
            _wikiImporter = wikiImporter;
            _repository = repository;
            _pluginFactory = pluginFactory;
            _wikiExporter = wikiExporter;
        }

        /// <summary>
        /// Displays the main tools page.
        /// </summary>
        public ActionResult Index()
        {
            return View(_repository.GetOrganisationNames());
        }

        public ActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Add(AddOrganisationRequestModel model)
        {
            if (ModelState.IsValid)
            {
                _repository.AddOrganisation(model.Organisation);
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
    }
}

