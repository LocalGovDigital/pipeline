using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace Roadkill.Core.Database
{
	/// <summary>
	/// Represents a page's meta data (not content) in the data store.
	/// </summary>
	public class Page : IDataStoreEntity
	{
		private Guid _objectId;

		/// <summary>
		/// Gets or sets the page's unique ID.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		/// <remarks>
		/// Reasons for using an int for the primary key:
		/// + Clustered PKs without using guid.comb
		/// + Nice URLs.
		/// - Losing the certainty of uniqueness like a guid
		/// - Oracle is not supported.
		/// </remarks>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the user who created the page.
		/// </summary>
		/// <value>
		/// The created by user.
		/// </value>
		public string CreatedBy { get; set; }

		/// <summary>
		/// Gets or sets the date the page was created on.
		/// </summary>
		/// <value>
		/// The created on date.
		/// </value>
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the user who last modified the page.
		/// </summary>
		/// <value>
		/// The user who modified the page.
		/// </value>
		public string ModifiedBy { get; set; }

		/// <summary>
		/// Gets or sets the date the page was last modified on.
		/// </summary>
		/// <value>
		/// The modified on.
		/// </value>
		public DateTime ModifiedOn { get; set; }

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

        /// <summary>
        /// The organisation ID
        /// </summary>
        public int OrgID { get; set; }

		/// <summary>
		/// Gets or sets the tags for the page, in the format "tag1,tag2,tag3" (no spaces between tags).
		/// </summary>
		public string Tags { get; set; }

		/// <summary>
		/// Gets or sets whether the page is locked for admin-only editing.
		/// </summary>
		public bool IsLocked { get; set; }

        /// <summary>
        /// The relationships of the project
        /// </summary>
        public List<Relationship> Relationships { get; set; }

		/// <summary>
		/// The unique id for this object - for use with document stores that require a unique id for storage.
		/// </summary>
		public Guid ObjectId
		{
			get { return _objectId; }
			set { _objectId = value; }
		}
	}
}
