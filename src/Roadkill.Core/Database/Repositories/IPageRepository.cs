﻿using System;
using System.Collections.Generic;
using System.Linq;
using Roadkill.Core.Converters;
using Roadkill.Core.Database.LightSpeed;
using Roadkill.Core.Hackney.Parameters;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Database
{
	public interface IPageRepository
	{
        PageContent AddNewPage(Page page, 
            string text, 
            string editedBy,
            DateTime editedOn, DateTime
            projectStart, DateTime projectEnd,
            bool projectEstimatedTime, string ProjectStatus,
            Phase2Params parameters,
            string ProjectLanguage, 
            int orgID);
        PageContent AddNewPageContentVersion(Page page, 
            string text,
            string editedBy, 
            DateTime editedOn,
            int version,
            DateTime projectStart,
            DateTime projectEnd, 
            bool projectEstimatedTime,
            string ProjectStatus,
            Phase2Params parameters,
            string ProjectLanguage,
            int orgID);
        /// <summary>
        /// Returns a list of tags for all pages. Each item is a list of tags seperated by ,
        /// e.g. { "tag1, tag2, tag3", "blah, blah2" } 
        /// </summary>
        /// <returns></returns>
        IQueryable<PageEntity> Query();
	    IEnumerable<Page> AllPages();
        IEnumerable<PageContent> AllPageContents();
		IEnumerable<string> AllTags();
		void DeletePage(Page page);
		/// <summary>
		/// Removes a single version of page contents by its id.
		/// </summary>
		/// <param name="pageContent"></param>
		void DeletePageContent(PageContent pageContent);
		void DeleteAllPages();
		IEnumerable<Page> FindPagesCreatedBy(string username);
		IEnumerable<Page> FindPagesModifiedBy(string username);
		IEnumerable<Page> FindPagesContainingTag(string tag);
		IEnumerable<PageContent> FindPageContentsByPageId(int pageId);
		IEnumerable<PageContent> FindPageContentsEditedBy(string username);
		PageContent GetLatestPageContent(int pageId);
		Page GetPageById(int id);
		/// <summary>
		/// Case insensitive search by page title
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		Page GetPageByTitle(string title);
		PageContent GetPageContentById(Guid id);
		PageContent GetPageContentByPageIdAndVersionNumber(int id, int versionNumber);
		IEnumerable<PageContent> GetPageContentByEditedBy(string username);
		Page SaveOrUpdatePage(Page page);
		void UpdatePageContent(PageContent content); // no new version
        IEnumerable<Activity> ActivityViewList();
 	}
}
