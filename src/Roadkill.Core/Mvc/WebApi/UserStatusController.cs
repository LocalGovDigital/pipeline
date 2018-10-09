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
    [RoutePrefix("api/user-status")]
    public class UserStatusController : ApiControllerBase
    {
        private readonly SearchService _searchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchController"/> class.
        /// </summary>
        /// <param name="searchService">The search service.</param>
        public UserStatusController(SearchService searchService, ApplicationSettings appSettings, UserServiceBase userService, IUserContext userContext)
            : base(appSettings, userService, userContext)
        {
            _searchService = searchService;
        }


        [HttpPost]
        [Route("request-watch/{projectid}")]
        public bool RequestWatch(int projectid)
        {

            var username = UserContext.CurrentUsername;

            ApplicationSettings appSettings = new ApplicationSettings();
            appSettings.DataStoreType = DataStoreType.Sqlite;
            appSettings.ConnectionString = "Data Source=|DataDirectory|\roadkill.sqlite;";
            appSettings.LoggingTypes = "none";
            appSettings.UseBrowserCache = false;

            LightSpeedRepository repository = new LightSpeedRepository(appSettings);
            var organisation = repository.GetOrgByUser(username);

            if (repository.IsWatched(projectid, username))
            {

                repository.UnWatchProject(projectid, username);

            }
            else
            {

                repository.WatchProject(projectid, username, organisation?.Id ?? 0);


            }

            return repository.IsWatched(projectid, username);
        }

        [HttpGet]
        [Route("watch-status/{projectid}")]
        public bool WatchStatus(int projectid)
        {

            var username = string.Empty;

            ApplicationSettings appSettings = new ApplicationSettings();
            appSettings.DataStoreType = DataStoreType.Sqlite;
            appSettings.ConnectionString = "Data Source=|DataDirectory|\roadkill.sqlite;";
            appSettings.LoggingTypes = "none";
            appSettings.UseBrowserCache = false;

            LightSpeedRepository repository = new LightSpeedRepository(appSettings);
            return repository.Rels.Any(x => x.username == username && x.pageId == projectid && x.relTypeId == 3);
        }

        [HttpPost]
        [Route("request-contribute/{projectid}")]
        public bool RequestContribute(int projectid)
        {
            var username = UserContext.CurrentUsername;

            ApplicationSettings appSettings = new ApplicationSettings();
            appSettings.DataStoreType = DataStoreType.Sqlite;
            appSettings.ConnectionString = "Data Source=|DataDirectory|\roadkill.sqlite;";
            appSettings.LoggingTypes = "none";
            appSettings.UseBrowserCache = false;

            LightSpeedRepository repository = new LightSpeedRepository(appSettings);

            var organisation = repository.GetOrgByUser(username);
            if (!repository.IsContributePending(projectid, username))
            {
                repository.SetPendingApprovedInProject(projectid, username, organisation?.Id ?? 0);
            }

            return repository.IsContributePending(projectid, username);
        }

        [HttpPost]
        [Route("approve-contribute/{projectid}/{relid}")]
        public bool ApproveContribute(int projectid, int relid)
        {
            if (UserContext.IsAdmin)
            {
                ApplicationSettings appSettings = new ApplicationSettings();
                appSettings.DataStoreType = DataStoreType.Sqlite;
                appSettings.ConnectionString = "Data Source=|DataDirectory|\roadkill.sqlite;";
                appSettings.LoggingTypes = "none";
                appSettings.UseBrowserCache = false;

                LightSpeedRepository repository = new LightSpeedRepository(appSettings);

                if (!repository.IsContributeApproved(projectid, relid))
                {
                    repository.SetContributeApprovedInProject(projectid, relid);
                }

                return true;
            }
            return false;
        }

        [HttpPost]
        [Route("reject-contribute/{projectid}/{relid}")]
        public bool RejectContribute(int projectid, int relid)
        {
            if (UserContext.IsAdmin)
            {
                ApplicationSettings appSettings = new ApplicationSettings();
                appSettings.DataStoreType = DataStoreType.Sqlite;
                appSettings.ConnectionString = "Data Source=|DataDirectory|\roadkill.sqlite;";
                appSettings.LoggingTypes = "none";
                appSettings.UseBrowserCache = false;

                LightSpeedRepository repository = new LightSpeedRepository(appSettings);

                if (!repository.IsContributeApproved(projectid, relid))
                {
                    repository.SetContributeRejectedInProject(projectid, relid);
                }

                return true;
            }
            return false;
        }

        [HttpGet]
        [Route("can-contribute/{projectid}")]
        public string CanContribute(int projectid)
        {

            var username = string.Empty;

            ApplicationSettings appSettings = new ApplicationSettings();
            appSettings.DataStoreType = DataStoreType.Sqlite;
            appSettings.ConnectionString = "Data Source=|DataDirectory|\roadkill.sqlite;";
            appSettings.LoggingTypes = "none";
            appSettings.UseBrowserCache = false;

            LightSpeedRepository repository = new LightSpeedRepository(appSettings);

            if (repository.Rels.Any(x => x.username == username && x.pageId == projectid && x.relTypeId == 4))
            {
                return "notrequested";

            }
            if (repository.Rels.Any(x => x.username == username && x.pageId == projectid && x.relTypeId == 4 && x.Approved == false))
            {
                return "requested";

            }
            if (repository.Rels.Any(x => x.username == username && x.pageId == projectid && x.relTypeId == 4 && x.Approved == false))
            {
                return "notapproved";
            }
            if (repository.Rels.Any(x => x.username == username && x.pageId == projectid && x.relTypeId == 4 && x.Approved == false))
            {
                return "approved";
            }

            throw new NotImplementedException();
        }
    }
}
