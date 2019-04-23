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
using StructureMap;

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
        public UserStatusController(SearchService searchService, ApplicationSettings appSettings,
            UserServiceBase userService, IUserContext userContext)
            : base(appSettings, userService, userContext)
        {
            _searchService = searchService;
        }


        [HttpPost]
        [Route("request-watch/{projectid}")]
        public bool RequestWatch(int projectid)
        {

            var userId = UserContext.CurrentUserId;
            LightSpeedRepository repository =
                new LightSpeedRepository(ObjectFactory.GetInstance<ApplicationSettings>());

            var organisation = repository.GetOrgByUser(userId);

            if (repository.IsWatched(projectid, userId))
            {

                repository.UnWatchProject(projectid, userId);

            }
            else
            {

                repository.WatchProject(projectid, userId, organisation?.Id ?? 0);


            }

            return repository.IsWatched(projectid, userId);
        }

        [HttpGet]
        [Route("watch-status/{projectid}")]
        public bool WatchStatus(int projectid)
        {

            var userid = UserContext.CurrentUserId;
            LightSpeedRepository repository =
                new LightSpeedRepository(ObjectFactory.GetInstance<ApplicationSettings>());

            return repository.Rels.Any(x => x.UserId == userid && x.pageId == projectid && x.relTypeId == 3);
        }

        [HttpPost]
        [Route("request-contribute/{projectid}")]
        public bool RequestContribute(int projectid)
        {
            var userId = UserContext.CurrentUserId;

            LightSpeedRepository repository =
                new LightSpeedRepository(ObjectFactory.GetInstance<ApplicationSettings>());

            var organisation = repository.GetOrgByUser(userId);
            if (!repository.IsContributePending(projectid, userId))
            {
                repository.SetPendingApprovedInProject(projectid, userId, organisation?.Id ?? 0);
            }

            return repository.IsContributePending(projectid, userId);
        }

        [HttpPost]
        [Route("approve-contribute/{projectid}/{relid}")]
        public bool ApproveContribute(int projectid, int relid)
        {
            if (UserContext.IsAdmin)
            {
                LightSpeedRepository repository =
                    new LightSpeedRepository(ObjectFactory.GetInstance<ApplicationSettings>());

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
                LightSpeedRepository repository =
                    new LightSpeedRepository(ObjectFactory.GetInstance<ApplicationSettings>());

                if (!repository.IsContributeApproved(projectid, relid))
                {
                    repository.SetContributeRejectedInProject(projectid, relid);
                }

                return true;
            }

            return false;
        }
        

        [HttpGet]
        [Route("update-database/{u}/{password}")]
        public int UpdateDatabase(string u, string password)
        {

            var count = 0;

            if (u == "updg0022" && password == "C6MGRkq4AhHutyEeaT3S7bgrxnVUjpzDWwZcKNP9v8fmJ2L5Bs")
            {
                LightSpeedRepository repository =
                    new LightSpeedRepository(ObjectFactory.GetInstance<ApplicationSettings>());

                foreach (var rel in repository.Rels)
                {
                    var user = repository.Users.FirstOrDefault(x => x.Username == rel.username);

                    if (user != null)
                    {
                        if (user.Id != Guid.Empty)
                        {
                            rel.UserId = user.Id;
                            repository.UnitOfWork.SaveChanges();
                            count++;
                        }
                    }
                }
                return count;
            }

            return count;
        }
    }
}
