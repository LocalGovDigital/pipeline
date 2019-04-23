using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using Roadkill.Core.Database.LightSpeed;
using Roadkill.Core.Email;
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
        private readonly PageService _pageService;
        private readonly ProjectStatusUpdateEmail _projectStatusUpdateEmail;
        public ProjectStatusController(SearchService searchService, PageService pageService, ApplicationSettings appSettings, UserServiceBase userService, IUserContext userContext, ProjectStatusUpdateEmail projectStatusUpdateEmail)
            : base(appSettings, userService, userContext)
        {
            _searchService = searchService;
            _pageService = pageService;
            _projectStatusUpdateEmail = projectStatusUpdateEmail;
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
        public StatusUpdateViewModel Post([FromBody] StatusUpdateViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            var appSettings = AppSettings();

            LightSpeedRepository repository = new LightSpeedRepository(appSettings);

            model.Author = UserContext.CurrentUserFullName;
            var updatedStatus = repository.CreateStatusUpdate(model);


            var pageContents = _pageService.GetById(model.PageId);
            SendUpdate(repository, updatedStatus, model.PageId, model.PageId, pageContents.Title, _projectStatusUpdateEmail);
            return updatedStatus;
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public int Delete([FromUri] int id)
        {
            var appSettings = AppSettings();
            LightSpeedRepository repository = new LightSpeedRepository(appSettings);
            repository.DeleteStatusUpdate(id);

            return id;


        }


        public void SendUpdate(LightSpeedRepository repository, StatusUpdateViewModel updatedStatus, int pageId, int projectId, string projectTitle, ProjectStatusUpdateEmail projectStatusUpdateEmail)
        {

            var relationList = _pageService.GetRelationsByPageId(pageId).ToList();
            var emailModels = new List<EmailProjectStatusUpdateViewModel>();

            var environmentPrefix = string.Empty;
            if (ConfigurationManager.AppSettings.AllKeys.Contains("environment"))
            {
                environmentPrefix = ConfigurationManager.AppSettings["environment"];
                environmentPrefix = $"[{environmentPrefix}] ";
            }

            foreach (var relViewModel in relationList)
            {
                var mod = new EmailProjectStatusUpdateViewModel
                {
                    ProjectName = projectTitle,
                    ProjectId = projectId.ToString(),
                    Subject = $"{environmentPrefix}The status of project [{projectTitle}] has been updated.",
                    Author = updatedStatus.Author,
                    UpdateDate = updatedStatus.UpdateDate.ToString("f"),
                    StatusUpdate = updatedStatus.Text
                };

                var user = repository.GetUserById(relViewModel.userId);

                if (user != null)
                {
                    mod.Name = $"{user.Firstname} {user.Lastname}";
                    mod.ToAddress = user.Email;
                }

                emailModels.Add(mod);
                projectStatusUpdateEmail.Send(mod);
            }
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
