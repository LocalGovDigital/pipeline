using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Roadkill.Core.Configuration;
using Roadkill.Core.Models;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Core.Security;
using Roadkill.Core.Services;

namespace Roadkill.Core.Mvc.Controllers
{
    [OptionalAuthorization]
    public class SearchController : ControllerBase
    {
        public PageService PageService { get; private set; }

        public SearchController(ApplicationSettings settings, UserServiceBase userService, PageService pageService, IUserContext context, SettingsService settingsService) : base(settings, userService, context, settingsService)
        {
            PageService = pageService;
        }


        public ActionResult Index()
        {
            var sp = ProjectSearchParameters.FromQuery(Request.QueryString);

            ViewBag.Title = "Search";

            var results = PageService.Search(sp);


            results.Organisations = PageService.GetOrganisationNames();
            results.Phases = new string[] { "Concept", "Discovery", "Alpha", "Beta", "Live", "Decommissioned" };
            results.AgileLifeCycleItems = new string[] { "All", "Discover", "Prototype", "Build", "Improve" };
            return View(results);
        }

    }
}
