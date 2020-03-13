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
using Roadkill.Core.Text;
using System.Text;

namespace Roadkill.Core.Mvc.Controllers
{
    /// <summary>
    /// Provides functionality for the tools page for admins.
    /// </summary>
    /// <remarks>All actions in this controller require admin rights.</remarks>
    [AdminRequired]
    public class ReportingController : ControllerBase
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

        public ReportingController(ApplicationSettings settings, UserServiceBase userManager,
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

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
            public ActionResult Export()
            {
                var sp = Roadkill.Core.Models.ProjectSearchParameters.FromQuery(new System.Collections.Specialized.NameValueCollection());

                var results = _pageService.Search(sp).Result;

                StringBuilder sb = new StringBuilder();

                sb.AppendLine($"Title, Organisation, Department, Owner, Owner email, Project start date, Project end date, Last updated");
                sb.AppendLine();

                foreach (var result in results) {
                    sb.AppendLine($"{AddDoubleQuotes(result.Title)}, {AddDoubleQuotes(result.Organisation)}, {AddDoubleQuotes(result.Department)}, {AddDoubleQuotes(result.Owner)}, {AddDoubleQuotes(result.OwnerEmail)}, {result.ProjectStart.ToShortDateString()}, {result.ProjectEnd.ToShortDateString()}, {result.LastUpdated.ToShortDateString()}");
                }

                var outputString = sb.ToString();
                var array = System.Text.Encoding.UTF8.GetBytes(outputString);
                var memorystream = new MemoryStream(array);

                return File(memorystream, "application/octet-stream", "PipelineReports.csv");
            }

            private string AddDoubleQuotes(string text) {
                if (text == null) {
                    return string.Empty;
                }
                if (text.Contains(',')) {
                    text = text.Replace(System.Environment.NewLine, string.Empty).Replace(",", string.Empty);
                }
                return text;
            }
        
    }
}

