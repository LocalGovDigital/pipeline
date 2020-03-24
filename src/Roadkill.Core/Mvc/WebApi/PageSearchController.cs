using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using Roadkill.Core.Database.LightSpeed;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Security;
using Roadkill.Core.Services;

namespace Roadkill.Core.Mvc.Controllers.Api
{
    [RoutePrefix("api/page-search")]
    public class PageSearchController : ApiControllerBase
    {
        private readonly SearchService _searchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchController"/> class.
        /// </summary>
        /// <param name="searchService">The search service.</param>
        public PageSearchController(SearchService searchService, ApplicationSettings appSettings, UserServiceBase userService, IUserContext userContext)
            : base(appSettings, userService, userContext)
        {
            _searchService = searchService;
        }


        [HttpGet]
        [Route("search-ahead/{query}")]
        public List<string> Get(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return new List<string>();
            }

            ApplicationSettings appSettings = new ApplicationSettings();
            appSettings.DataStoreType = DataStoreType.Sqlite;
            appSettings.ConnectionString = "Data Source=|DataDirectory|\roadkill.sqlite;";
            appSettings.LoggingTypes = "none";
            appSettings.UseBrowserCache = false;

            LightSpeedRepository repository = new LightSpeedRepository(appSettings);
            return repository.Pages.Where(x => x.Title.ToLower().Contains(query.ToLower())).Select(x => x.Title).ToList();

        }



        [HttpGet]
        [Route("search-ahead/org/{query}")]
        public List<string> OrganisationSearch(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return new List<string>();
            }

            ApplicationSettings appSettings = new ApplicationSettings();
            appSettings.DataStoreType = DataStoreType.Sqlite;
            appSettings.ConnectionString = "Data Source=|DataDirectory|\roadkill.sqlite;";
            appSettings.LoggingTypes = "none";
            appSettings.UseBrowserCache = false;

            LightSpeedRepository repository = new LightSpeedRepository(appSettings);
            return repository.Orgs.Where(x => x.OrgName.ToLower().Contains(query.ToLower())).Select(x => x.OrgName).ToList();

        }
    }
}
