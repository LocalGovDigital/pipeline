using System;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Linq;
using Roadkill.Core.Configuration;
using Roadkill.Core.Services;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Security;
using System.Collections.Generic;
using Microsoft.Ajax.Utilities;

namespace Roadkill.Core.Mvc.Controllers
{
    /// <summary>
    /// Provides functionality for the /wiki/{id}/{title} route, which all pages are displayed via.
    /// </summary>
    [OptionalAuthorization]
    public class WikiController : ControllerBase
    {
        public PageService PageService { get; private set; }
        private readonly SearchService _searchService;

        public WikiController(ApplicationSettings settings, UserServiceBase userManager, PageService pageService,
            IUserContext context, SettingsService settingsService, SearchService searchService)
            : base(settings, userManager, context, settingsService)
        {
            PageService = pageService;
            _searchService = searchService;

        }

        /// <summary>
        /// Displays the wiki page using the provided id.
        /// </summary>
        /// <param name="id">The page id</param>
        /// <param name="title">This parameter is passed in, but never used in queries.</param>
        /// <returns>A <see cref="PageViewModel"/> to the Index view.</returns>
        /// <remarks>This action adds a "Last-Modified" header using the page's last modified date, if no user is currently logged in.</remarks>
        /// <exception cref="HttpNotFoundResult">Thrown if the page with the id cannot be found.</exception>
        [BrowserCache]
        public ActionResult Index(int? id, string title)
        {
            if (id == null || id < 1)
                return RedirectToAction("Index", "Home");

            PageViewModel model = PageService.GetById(id.Value, true);

            if (model != null)
            {
                var tags = new List<Tuple<string, int>>();

                foreach (var tag in PageService.AllTags())
                {
                    if (model.Tags.Contains(tag.Name)) tags.Add(new Tuple<string, int>(tag.Name, tag.Count));
                }

                model.TagCloud = tags;
                model.FundingBoundaryText = PageService.GetFundingBoundaryText(model.FundingBoundary);

            }

            if (model == null)
                throw new HttpException(404, string.Format("The page with id '{0}' could not be found", id));

         
            return View(model);
        }

        public ActionResult PageToolbar(int? id)
        {
            if (id == null || id < 1)
                return Content("");

            PageViewModel model = PageService.GetById(id.Value);

            if (model == null)
                return Content(string.Format("The page with id '{0}' could not be found", id));

            return PartialView(model);
        }

        public ActionResult RelatedRelationships(int id)
        {
            if (id == null || id < 1)
                return Content("");

            IEnumerable<RelViewModel> model = PageService.GetRelByPage(id);

            if (model == null)
                return Content(string.Format("The page with id '{0}' could not be found", id));

            return PartialView(model);
        }

        public ActionResult Activity()
        {
            IEnumerable<ActivityViewModel> model = PageService.GetActivity();

            if (model == null)
                return Content(string.Format("No activity could not be found"));

            return PartialView(model);
        }

        /// <summary>
        /// 404 not found page - configured in the web.config
        /// </summary>
        public ActionResult NotFound()
        {
            return View("404");
        }

        /// <summary>
        /// 500 internal error - configured in the web.config
        /// </summary>
        public ActionResult ServerError()
        {
            return View("500");
        }

        public ActionResult Forbidden()
        {
            return View("403");
        }
    }
}
