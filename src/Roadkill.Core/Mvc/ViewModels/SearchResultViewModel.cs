using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Lucene.Net.Documents;
using Lucene.Net.Search;

namespace Roadkill.Core.Mvc.ViewModels
{
	/// <summary>
	/// Contains data for a single search result from a search query.
	/// </summary>
	public class SearchResultViewModel
	{
		/// <summary>
		/// The page id 
		/// </summary>
		public int Id { get; internal set; }

		/// <summary>
		/// The page title.
		/// </summary>
		public string Title { get; internal set; }

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
		/// The summary of the content (the first 150 characters of text with all HTML removed).
		/// </summary>
		public string ContentSummary { get; internal set; }

		/// <summary>
		/// The length of the content in bytes.
		/// </summary>
		public int ContentLength { get; internal set; }

		// TODO: tests
		/// <summary>
		/// Formats the page length in bytes using KB or bytes if it is less than 1024 bytes.
		/// </summary>
		/// <param name="helper">The helper.</param>
		/// <param name="size">The size in bytes.</param>
		/// <returns>If the size parameter is 900: 900 bytes. If size is 4000: 4KB.</returns>
		public string ContentLengthInKB
		{
			get
			{
				if (ContentLength > 1024)
					return ContentLength / 1024 + "KB";
				else
					return ContentLength + " bytes";
			}
		}

		/// <summary>
		/// The person who created the page.
		/// </summary>
		public string CreatedBy { get; internal set; }

		/// <summary>
		/// The date the page was created on.
		/// </summary>
		public DateTime CreatedOn { get; internal set; }

		/// <summary>
		/// The tags for the page, in space delimited format.
		/// </summary>
		public string Tags { get; internal set; }

		/// <summary>
		/// The lucene.net score for the search result.
		/// </summary>
		public float Score { get; internal set; }

        /// <summary>
        /// Gets or sets the date the project is due to start.
        /// </summary>
        /// <value>
        /// The project start.
        /// </value>
        ///
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ProjectStart { get; set; }

        /// <summary>
        /// Gets or sets the date the project is due to end.
        /// </summary>
        /// <value>
        /// The project end.
        /// </value>
        ///
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ProjectEnd { get; set; }

        /// <summary>
        /// Gets or sets whether the start and end dates are estimated.
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


		public SearchResultViewModel()
		{
		}

		public SearchResultViewModel(Document document, ScoreDoc scoreDoc)
		{
			if (document == null)
				throw new ArgumentNullException("document");

			if (scoreDoc == null)
				throw new ArgumentNullException("scoreDoc");

			EnsureFieldsExist(document);

			Id = int.Parse(document.GetField("id").StringValue);
			Title = document.GetField("title").StringValue;
			ContentSummary = document.GetField("contentsummary").StringValue;
			Tags = document.GetField("tags").StringValue;
			CreatedBy = document.GetField("createdby").StringValue;		
			ContentLength = int.Parse(document.GetField("contentlength").StringValue);
			Score = scoreDoc.Score;
            ProjectStatus = document.GetField("projectstatus").StringValue;
            ProjectLanguage = document.GetField("projectlanguage").StringValue;

            DateTime createdOn = DateTime.UtcNow;
            if (!DateTime.TryParse(document.GetField("createdon").StringValue, out createdOn))
                createdOn = DateTime.UtcNow;

            CreatedOn = createdOn;

            DateTime projectStart = DateTime.UtcNow;
            if (!DateTime.TryParse(document.GetField("projectstart").StringValue, out projectStart))
                projectStart = DateTime.UtcNow;

            ProjectStart = projectStart;

            DateTime projectEnd = DateTime.UtcNow;
            if (!DateTime.TryParse(document.GetField("projectend").StringValue, out projectEnd))
                projectEnd = DateTime.UtcNow;

			ProjectEnd = projectEnd;
		}

		private void EnsureFieldsExist(Document document)
		{
			IList<IFieldable> fields = document.GetFields();
			EnsureFieldExists(fields, "id");
			EnsureFieldExists(fields, "title");
			EnsureFieldExists(fields, "contentsummary");
			EnsureFieldExists(fields, "tags");
			EnsureFieldExists(fields, "createdby");
			EnsureFieldExists(fields, "contentlength");
            EnsureFieldExists(fields, "createdon");
            EnsureFieldExists(fields, "projectstart");
            EnsureFieldExists(fields, "projectend");
            EnsureFieldExists(fields, "projectestimatedtime");
            EnsureFieldExists(fields, "projectstatus");
            EnsureFieldExists(fields, "projectlanguage");
		}

		private void EnsureFieldExists(IList<IFieldable> fields, string fieldname)
		{
			if (fields.Any(x => x.Name == fieldname) == false)
				throw new SearchException(null, "The LuceneDocument did not contain the expected field '{0}'", fieldname);
		}

		/// <summary>
		/// Used by the search results view, for the list of tags.
		/// </summary>
		public IEnumerable<string> TagsAsList()
		{
			List<string> tags = new List<string>();

			if (!string.IsNullOrEmpty(Tags))
			{
				if (Tags.IndexOf(" ") != -1)
				{
					string[] parts = Tags.Split(' ');
					foreach (string item in parts)
					{
						if (item != " ")
							tags.Add(item);
					}
				}
				else
				{
					tags.Add(Tags.TrimEnd());
				}
			}

			return tags;
		}
	}
}
