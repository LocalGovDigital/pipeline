﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Roadkill.Core.Localization;
using Roadkill.Core.Database;
using Roadkill.Core.Text;
using Roadkill.Core.Converters;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using Roadkill.Core.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Roadkill.Core.Database.LightSpeed;
using Roadkill.Core.Mvc.Controllers.Api;
using System.Configuration;
using System.Net.Http;
using Roadkill.Core.Mvc.ViewModels;
using System.Web.Configuration;


namespace Roadkill.Core.Mvc.ViewModels
{
	/// <summary>
	/// Provides summary data for a page.
	/// </summary>
	[CustomValidation(typeof(PageViewModel), "VerifyRawTags")]
	public class PageViewModel
	{
		private static string[] _tagBlackList = 
		{
			"#", ";", "/", "?", ":", "@", "&", "=", "{", "}", "|", "\\", "^", "[", "]", "`"		
		};

		private List<string> _tags;
		private string _rawTags;
		private string _content;

		/// <summary>
		/// The page's unique id.
		/// </summary>
		public int Id { get; set; }
		
		/// <summary>
		/// The text content for the page.
		/// </summary>
		public string Content
		{
			get { return _content; }
			set
			{
				// Ensure the content isn't null for lucene's benefit
				_content = value;
				if (_content == null)
					_content = "";
			}
		}

		/// <summary>
		/// The content after it has been transformed into HTML by the current wiki markup converter. This property 
		/// is only set when the PageContent object is passed into the constructor, and is empty unless explicitly 
		/// set by the caller.
		/// </summary>
		public string ContentAsHtml { get; set; }

		/// <summary>
		/// The user who created the page.
		/// </summary>
		public string CreatedBy { get; set; }

		/// <summary>
		/// The date the page was created.
		/// </summary>
		public DateTime CreatedOn { get; set; }



		/// <summary>
		/// Returns true if no Id exists for the page.
		/// </summary>
		public bool IsNew
		{
			get
			{
				return Id == 0;
			}
		}

		/// <summary>
		/// The user who last modified the page.
		/// </summary>
		public string ModifiedBy { get; set; }

		/// <summary>
		/// The date the page was last modified on.
		/// </summary>
		public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// The date the page was last modified on.
        /// </summary>
        public DateTime ProjectStart { get; set; }

        /// <summary>
        /// The date the page was last modified on.
        /// </summary>
        public DateTime ProjectEnd { get; set; }

        /// <summary>
        /// The date the page was last modified on.
        /// </summary>
        public bool ProjectEstimatedTime { get; set; }

        /// <summary>
        /// The status of the project
        /// </summary>
        public string ProjectStatus { get; set; }

        /// <summary>
        /// The main language of the project
        /// </summary>
        public string ProjectLanguage { get; set; }

        /// <summary>
        /// The main language of the project
        /// </summary>
        public int OrgID { get; set; }

		/// <summary>
		/// Displays ModifiedOn in IS8601 format, plus the timezone offset included for timeago
		/// </summary>
		public string ModifiedOnWithOffset
		{
			get
			{
				// EditedOn (ModifiedOn in the domain) is stored in UTC time, so just add a Z to indicate this.
				return string.Format("{0}Z", ModifiedOn.ToString("s"));
			}
		}
		
		/// <summary>
		/// Gets the tags for the page as a list.
		/// </summary>
		public IEnumerable<string> Tags
		{
			get { return _tags; }
		}

		/// <summary>
		/// Sets or gets the tags for the page - these should be in comma separated format.
		/// </summary>
		public string RawTags
		{
			get 
			{ 
				return _rawTags; 
			}
			set
			{
				_rawTags = value;
				ParseRawTags();
			}
		}

		/// <summary>
		/// The page title before any update.
		/// </summary>
		public string PreviousTitle { get; set; }

		/// <summary>
		/// The page title.
		/// </summary>
		[Required(ErrorMessageResourceType=typeof(SiteStrings), ErrorMessageResourceName="Page_Validation_Title")]
		public string Title { get; set; }

		/// <summary>
		/// The page title, encoded so it is a safe search-engine friendly url.
		/// </summary>
		public string EncodedTitle
		{
			get
			{
				return PageViewModel.EncodePageTitle(Title);
			}
		}
		
		/// <summary>
		/// The current version number for the page.
		/// </summary>
		public int VersionNumber { get; set; }

		/// <summary>
		/// Whether the page has been locked so that only admins can edit it.
		/// </summary>
		public bool IsLocked { get; set; }

		/// <summary>
		/// Determines if the summary object can be cached on the browser and in the object cache. 
		/// This is true by default, but plugins that run on a page can mark a page as not cacheable.
		/// </summary>
		public bool IsCacheable { get; set; }

		/// <summary>
		/// Any additional head tag HTML generated by the text plugins.
		/// </summary>
		public string PluginHeadHtml { get; set; }

		/// <summary>
		/// Any additional footer HTML generated by the text plugins.
		/// </summary>
		public string PluginFooterHtml { get; set; }

		/// <summary>
		///  Any additional HTML generated by the text plugins that sits before the #container element.
		/// </summary>
		public string PluginPreContainer { get; set; }

		/// <summary>
		///  Any additional HTML generated by the text plugins that sits before the #container element.
		/// </summary>
		public string PluginPostContainer { get; set; }

		/// <summary>
		/// Retrieves all tags for all pages in the system. This is empty unless filled by the controller.
		/// </summary>
		[XmlIgnore]
		public List<TagViewModel> AllTags { get; set; }

		public PageViewModel()
		{
			_tags = new List<string>();
			IsCacheable = true;
			PluginHeadHtml = "";
			PluginFooterHtml = "";
			PluginPreContainer = "";
			PluginPostContainer = "";
			AllTags = new List<TagViewModel>();
		}

		public PageViewModel(Page page)
		{
			if (page == null)
				throw new ArgumentNullException("page");

			Id = page.Id;
			Title = page.Title;
			PreviousTitle = page.Title;
			CreatedBy = page.CreatedBy;
			CreatedOn = page.CreatedOn;
			IsLocked = page.IsLocked;
			ModifiedBy = page.ModifiedBy;
			ModifiedOn = page.ModifiedOn;
			RawTags = page.Tags;
            ProjectStart = page.ProjectStart;
            ProjectEnd = page.ProjectEnd;
            ProjectEstimatedTime = page.ProjectEstimatedTime;
            ProjectStatus = page.ProjectStatus;
            ProjectLanguage = page.ProjectLanguage;
            OrgID = page.OrgID;

			CreatedOn = DateTime.SpecifyKind(CreatedOn, DateTimeKind.Utc);
			ModifiedOn = DateTime.SpecifyKind(ModifiedOn, DateTimeKind.Utc);
			AllTags = new List<TagViewModel>();
		}

		public PageViewModel(PageContent pageContent, MarkupConverter converter)
		{
			if (pageContent == null)
				throw new ArgumentNullException("pageContent");

			if (pageContent.Page == null)
				throw new ArgumentNullException("pageContent.Page");

			if (converter == null)
				throw new ArgumentNullException("converter");

			Id = pageContent.Page.Id;
			Title = pageContent.Page.Title;
			PreviousTitle = pageContent.Page.Title;
			CreatedBy = pageContent.Page.CreatedBy;
			CreatedOn = pageContent.Page.CreatedOn;
			IsLocked = pageContent.Page.IsLocked;
			ModifiedBy = pageContent.Page.ModifiedBy;
			ModifiedOn = pageContent.Page.ModifiedOn;
			RawTags = pageContent.Page.Tags;
			Content = pageContent.Text;
			VersionNumber = pageContent.VersionNumber;
            ProjectStart = pageContent.Page.ProjectStart;
            ProjectEnd = pageContent.ProjectEnd;
            ProjectEstimatedTime = pageContent.ProjectEstimatedTime;
            ProjectStatus = pageContent.ProjectStatus;
            ProjectLanguage = pageContent.ProjectLanguage;
            OrgID = pageContent.OrgID;

			PageHtml pageHtml = converter.ToHtml(pageContent.Text);
			ContentAsHtml = pageHtml.Html;
			IsCacheable = pageHtml.IsCacheable;
			PluginHeadHtml = pageHtml.HeadHtml;
			PluginFooterHtml = pageHtml.FooterHtml;
			PluginPreContainer = pageHtml.PreContainerHtml;
			PluginPostContainer = pageHtml.PostContainerHtml;

			CreatedOn = DateTime.SpecifyKind(CreatedOn, DateTimeKind.Utc);
			ModifiedOn = DateTime.SpecifyKind(ModifiedOn, DateTimeKind.Utc);
			AllTags = new List<TagViewModel>();
		}

		/// <summary>
		/// Joins the parsed tags with a comma.
		/// </summary>
		public string CommaDelimitedTags()
		{
			return string.Join(",", _tags);
		}

		/// <summary>
		/// Formats the <see cref="AllTags"/> to insert inside an array initializer like [];
		/// </summary>
		public string JavascriptArrayForAllTags()
		{
			IEnumerable<string> allTags = AllTags.OrderBy(x => x.Name).Select(t => t.Name);
			return "\"" +string.Join("\", \"", allTags) + "\"";
		}

		/// <summary>
		/// Joins the tags with a space.
		/// </summary>
		public string SpaceDelimitedTags()
		{
			return string.Join(" ", _tags);
		}

		private void ParseRawTags()
		{
			_tags = ParseTags(_rawTags).ToList();
		}

		/// <summary>
		/// Takes a string of tags: "tagone,tagtwo,tag3 " and returns a list.
		/// </summary>
		/// <param name="tags"></param>
		/// <returns></returns>
		public static IEnumerable<string> ParseTags(string tags)
		{
			List<string> tagList = new List<string>();
			char delimiter = ',';

			if (!string.IsNullOrEmpty(tags))
			{
				// For the legacy tag seperator format
				if (tags.IndexOf(";") != -1)
					delimiter = ';';

				if (tags.IndexOf(delimiter) != -1)
				{
					string[] parts = tags.Split(delimiter);
					foreach (string item in parts)
					{
						if (item != "," && !string.IsNullOrWhiteSpace(item))
							tagList.Add(item.Trim());
					}
				}
				else
				{
					tagList.Add(tags.TrimEnd());
				}
			}

			return tagList;
		}

		/// <summary>
		/// Returns false if the tag contains ; / ? : @ & = { } | \ ^ [ ] `		
		/// </summary>
		/// <param name="tag">The tag</param>
		/// <returns></returns>
		public static ValidationResult VerifyRawTags(PageViewModel pageViewModel, ValidationContext context)
		{
			if (string.IsNullOrEmpty(pageViewModel.RawTags))
				return ValidationResult.Success;

			if (_tagBlackList.Any(x => pageViewModel.RawTags.Contains(x)))
			{
				return new ValidationResult("Invalid characters in tag"); // doesn't need to be localized as there's a javascript check
			}
			else
			{
				return ValidationResult.Success;
			}
		}

		// TODO: tests
		/// <summary>
		/// Removes all bad characters (ones which cannot be used in a URL for a page) from a page title.
		/// </summary>
		public static string EncodePageTitle(string title)
		{
			if (string.IsNullOrEmpty(title))
				return title;

			// Search engine friendly slug routine with help from http://www.intrepidstudios.com/blog/2009/2/10/function-to-generate-a-url-friendly-string.aspx

			// remove invalid characters
			title = Regex.Replace(title, @"[^\w\d\s-]", "");  // this is unicode safe, but may need to revert back to 'a-zA-Z0-9', need to check spec

			// convert multiple spaces/hyphens into one space       
			title = Regex.Replace(title, @"[\s-]+", " ").Trim();

			// If it's over 30 chars, take the first 30.
			title = title.Substring(0, title.Length <= 75 ? title.Length : 75).Trim();

			// hyphenate spaces
			title = Regex.Replace(title, @"\s", "-");

			return title;
		}


        /// <summary>
        /// Gets an IEnumerable{SelectListItem} of project statuses, as a default
        /// SelectList doesn't add option value attributes.
        /// </summary>
        public List<SelectListItem> ProjectStatusTypesAsSelectList
        {
          
           get{
                string[] strStatuses = new string[] { "Concept", "Discovery", "Alpha", "Beta", "Live", "Decommissioned" };                
                              
                List<SelectListItem> items = new List<SelectListItem>();

                foreach(string status in strStatuses)
                {

                    SelectListItem item = new SelectListItem();
                    item.Text = status;
                    item.Value = status;


                    if (ProjectStatus == status.ToString())
                    { item.Selected = true;
                    }

                    items.Add(item);
                }

                return items;
           }
           
        }


        /// <summary>
        /// Gets an IEnumerable{SelectListItem} of project statuses, as a default
        /// SelectList doesn't add option value attributes.
        /// </summary>
        public List<SelectListItem> LanguageTypesAsSelectList
        {
            get
            {
                string[] strLanguages = new string[] { "Non-dependant", "C#", "Java", "JavaScript", "PHP", "Ruby", "Other" };

                List<SelectListItem> items = new List<SelectListItem>();

                foreach (string language in strLanguages)
                {

                    SelectListItem item = new SelectListItem();
                    item.Text = language;
                    item.Value = language;

                    if (language == ProjectLanguage)
                    { item.Selected = true; }

                    items.Add(item);
                }

                return items;
            }
        }


        /// <summary>
        /// Gets an IEnumerable{SelectListItem} of project statuses, as a default
        /// SelectList doesn't add option value attributes.
        /// </summary>
        public List<SelectListItem> OrgsAsNewSelectList
        {
            get
            {

                ApplicationSettings appSettings = new ApplicationSettings();
                appSettings.DataStoreType = DataStoreType.Sqlite;
                appSettings.ConnectionString = "Data Source=|DataDirectory|\roadkill.sqlite;";
                appSettings.LoggingTypes = "none";
                appSettings.UseBrowserCache = false;

                LightSpeedRepository repository = new LightSpeedRepository(appSettings);

                IEnumerable<Organisation> OrgList;
                OrgList = repository.FindAllOrgs();

                List<SelectListItem> items = new List<SelectListItem>();

                int[] strLanguages = new int[] { 1, 2, 3, 4, 5 };

                foreach (Organisation org in OrgList)
                {

                    SelectListItem item = new SelectListItem();
                    item.Text = org.OrgName.ToString();
                    item.Value = org.Id.ToString();

                    items.Add(item);
                }

                items.Sort((x, y) => string.Compare(x.Text, y.Text));

                return items;
            }

        }
	}
}
