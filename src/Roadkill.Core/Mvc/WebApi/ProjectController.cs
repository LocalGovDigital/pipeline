using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Lucene.Net.Support;
using Roadkill.Core.Configuration;
using Roadkill.Core.Models;
using Roadkill.Core.Mvc.Controllers.Api;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Security;
using Roadkill.Core.Services;


namespace Roadkill.Core.Mvc.Controllers.Api
{
    [RoutePrefix("api/project")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class ProjectController : ApiControllerBase
    {
        private IPageService _pageService;
        private readonly SearchService _searchService;

        public ProjectController(SearchService searchService, IPageService pageService, ApplicationSettings appSettings, UserServiceBase userService, IUserContext userContext)
            : base(appSettings, userService, userContext)
        {
            _pageService = pageService;
            _searchService = searchService;
        }

        [HttpGet]
        [Route("{id}")]
        public ProjectViewModel GetProject(int id)
        {
            if (id == null || id < 1) return null;
            PageViewModel model = _pageService.GetById(id, true);
            if (model != null)
            {
                var tags = new List<Tuple<string, int>>();
                foreach (var tag in _pageService.AllTags())
                {
                    if (model.Tags.Contains(tag.Name)) tags.Add(new Tuple<string, int>(tag.Name, tag.Count));
                }
                model.TagCloud = tags;
                model.FundingBoundaryText = _pageService.GetFundingBoundaryText(model.FundingBoundary);
            }
            return ProjectViewModel.Create(model);
        }

        [HttpGet]
        [Route("")]
        public List<ProjectDetailSummary> GetProjects()
        {
            var sp = ProjectSearchParameters.FromQuery(new NameValueCollection());
            var list = _pageService.GetProjects(sp);


            return list;
        }
    }
}
