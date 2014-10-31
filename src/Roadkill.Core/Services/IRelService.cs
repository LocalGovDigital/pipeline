using System;
using System.Collections.Generic;
using Roadkill.Core.Database;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Services
{
	public interface IRelService
	{
		/// <summary>
		/// Adds the page to the database.
		/// </summary>
		/// <param name="model">The summary details for the page.</param>
		/// <returns>A <see cref="PageViewModel"/> for the newly added page.</returns>
		/// <exception cref="DatabaseException">An database error occurred while saving.</exception>
		/// <exception cref="SearchException">An error occurred adding the page to the search index.</exception>
        RelViewModel AddRel(RelViewModel model);

		/// <summary>
		/// Retrieves a list of all pages in the system.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{PageViewModel}"/> of the pages.</returns>
		/// <exception cref="DatabaseException">An database error occurred while retrieving the list.</exception>
        IEnumerable<RelViewModel> FindAllRels(bool loadPageContent = false);

		/// <summary>
		/// Gets alls the pages created by a user.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <returns>All pages created by the provided user, or an empty list if none are found.</returns>
		/// <exception cref="DatabaseException">An database error occurred while saving.</exception>
        IEnumerable<RelViewModel> AllRelsCreatedBy(string userName);


		/// <summary>
		/// Deletes a page from the database.
		/// </summary>
		/// <param name="pageId">The id of the page to remove.</param>
		/// <exception cref="DatabaseException">An database error occurred while deleting the page.</exception>
		void DeleteRel(int id);


		/// <summary>
		/// Retrieves the page by its id.
		/// </summary>
		/// <param name="id">The id of the page</param>
		/// <param name="loadContent">True if the page's content should also be loaded, which will also run all text plugins.</param>
		/// <returns>A <see cref="PageViewModel"/> for the page.</returns>
		/// <exception cref="DatabaseException">An database error occurred while getting the page.</exception>
        RelViewModel GetRelById(int id, bool loadContent = false);


		/// <summary>
		/// Updates the provided page.
		/// </summary>
		/// <param name="model">The page model.</param>
		/// <exception cref="DatabaseException">An database error occurred while updating.</exception>
		/// <exception cref="SearchException">An error occurred adding the page to the search index.</exception>
        void UpdateRel(RelViewModel model);


	}
}
