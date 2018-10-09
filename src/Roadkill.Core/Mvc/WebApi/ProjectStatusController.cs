using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
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
    [RoutePrefix("api/project-status")]
    public class ProjectStatusController : ApiControllerBase
    {
        private readonly SearchService _searchService;

        public ProjectStatusController(SearchService searchService, ApplicationSettings appSettings, UserServiceBase userService, IUserContext userContext)
            : base(appSettings, userService, userContext)
        {
            _searchService = searchService;
        }

        [HttpGet]
        [Route("list/{pageId}")]
        public IList<StatusUpdateViewModel> Get(int pageId)
        {
            if (pageId == 0)
            {
                return new List<StatusUpdateViewModel>();
            }


            var appSettings = AppSettings();
            LightSpeedRepository repository = new LightSpeedRepository(appSettings);
            return repository.GetStatusUpdates(pageId);
        }

        [HttpPost]
        [Route("update")]
        public IList<StatusUpdateViewModel> Post([FromBody] StatusUpdateViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            var appSettings = AppSettings();

            LightSpeedRepository repository = new LightSpeedRepository(appSettings);

            model.Author = "";
            repository.CreateStatusUpdate(model);
            return repository.GetStatusUpdates(model.PageId);
        }

        private static ApplicationSettings AppSettings()
        {
            ApplicationSettings appSettings = new ApplicationSettings();
            appSettings.DataStoreType = DataStoreType.Sqlite;
            appSettings.ConnectionString = "Data Source=|DataDirectory|\roadkill.sqlite;";
            appSettings.LoggingTypes = "none";
            appSettings.UseBrowserCache = false;
            return appSettings;
        }
    }
}
