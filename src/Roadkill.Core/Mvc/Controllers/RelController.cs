using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Roadkill.Core.Diff;
using Roadkill.Core.Converters;
using Roadkill.Core.Configuration;
using Roadkill.Core.Services;
using Roadkill.Core.Security;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Core.Mvc.ViewModels;
using System.Web;
using Roadkill.Core.Text;
using Roadkill.Core.Extensions;

namespace Roadkill.Core.Mvc.Controllers
{
	/// <summary>
	/// Provides all page related functionality, including editing and viewing pages.
	/// </summary>
	[HandleError]
	[OptionalAuthorization]
	public class RelController : ControllerBase
	{
		private SettingsService _settingsService;
		private IRelService _relService;

		public RelController(ApplicationSettings settings, UserServiceBase userManager,
			SettingsService settingsService, IRelService relService, SearchService searchService,
			PageHistoryService historyService, IUserContext context)
			: base(settings, userManager, context, settingsService)
		{
			_settingsService = settingsService;
            _relService = relService;
		}

		/// <summary>
		/// Displays a list of all page titles and ids in Roadkill.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{PageViewModel}"/> as the model.</returns>
		[BrowserCache]
		public ActionResult AllPages()
		{
			return View(_relService.AllRels());
		}



		/// <summary>
		/// Displays all pages for a particular user.
		/// </summary>
		/// <param name="id">The username</param>
		/// <param name="encoded">Whether the username paramter is Base64 encoded.</param>
		/// <returns>An <see cref="IEnumerable{PageViewModel}"/> as the model.</returns>
		public ActionResult ByUser(string id, bool? encoded)
		{
			// Usernames are base64 encoded by roadkill (to cater for usernames like domain\john).
			// However the URL also supports humanly-readable format, e.g. /ByUser/chris
			if (encoded == true)
			{
				id = id.FromBase64();
			}

			ViewData["Username"] = id;

			return View(_relService.AllRelsCreatedBy(id));
		}

		/// <summary>
		/// Deletes a wiki page.
		/// </summary>
		/// <param name="id">The id of the page to delete.</param>
		/// <returns>Redirects to AllPages action.</returns>
		/// <remarks>This action requires admin rights.</remarks>
		[AdminRequired]
		public ActionResult Delete(int id)
		{
			_relService.DeleteRel(id);

			return RedirectToAction("AllPages");
		}

		/// <summary>
		/// Displays the edit View for the page provided in the id.
		/// </summary>
		/// <param name="id">The ID of the page to edit.</param>
		/// <returns>An filled <see cref="PageViewModel"/> as the model. If the page id cannot be found, the action
		/// redirects to the New page.</returns>
		/// <remarks>This action requires editor rights.</remarks>
		[EditorRequired]
		public ActionResult Edit(int id)
		{
			RelViewModel model = _relService.GetRelById(id, true);

			if (model != null)
			{
                return PartialView("Relationship", model);
			}
			else
			{
				return RedirectToAction("New");
			}
		}

		/// <summary>
		/// Saves all POST'd data for a page edit to the database.
		/// </summary>
		/// <param name="model">A filled <see cref="PageViewModel"/> containing the new data.</param>
		/// <returns>Redirects to /Wiki/{id} using the passed in <see cref="PageViewModel.Id"/>.</returns>
		/// <remarks>This action requires editor rights.</remarks>
		[EditorRequired]
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Edit(RelViewModel model)
		{
			if (!ModelState.IsValid)
                return View("Relationship", "Relationship", model);

			_relService.UpdateRel(model);

			return RedirectToAction("Index", "Wiki", new { id = model.Id });
		}


		/// <summary>
		/// Displays the Edit view in new page mode.
		/// </summary>
		/// <returns>An empty <see cref="PageViewModel"/> as the model.</returns>
		/// <remarks>This action requires editor rights.</remarks>
		[EditorRequired]
        public ActionResult New(string pageID)
		{
            RelViewModel model = new RelViewModel();
            model.pageID = Convert.ToInt32(pageID);

            return PartialView("Relationship", model);
		}

		/// <summary>
		/// Saves a new page using the provided <see cref="PageViewModel"/> object to the database.
		/// </summary>
		/// <param name="model">The page details to save.</param>
		/// <returns>Redirects to /Wiki/{id} using the newly created page's ID.</returns>
		/// <remarks>This action requires editor rights.</remarks>
		[EditorRequired]
		[HttpPost]
		[ValidateInput(false)]
        public ActionResult New(RelViewModel model)
		{
			if (!ModelState.IsValid)
                return View("Relationship", "Relationship", model);

			model = _relService.AddRel(model);

			return RedirectToAction("Index", "Wiki", new { id = model.Id });
		}



	}
}