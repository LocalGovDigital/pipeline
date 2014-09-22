﻿using System;
using System.Text;
using System.Web.Security;
using System.Security.Cryptography;
using Roadkill.Core.Security;

namespace Roadkill.Core.Database
{
	/// <summary>
	/// A user object for use with the data store, whatever that might be (e.g. an RDMS or MongoDB)
	/// </summary>
	public class Organisation : IDataStoreEntity
	{

        private Guid _objectId;

        /// <summary>
        /// Gets or sets the page's unique ID.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string OrgName { get; set; }        

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