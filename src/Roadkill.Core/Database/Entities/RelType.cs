using System;
using System.Text;
using System.Web.Security;
using System.Security.Cryptography;
using Roadkill.Core.Security;

namespace Roadkill.Core.Database
{
	/// <summary>
	/// A user object for use with the data store, whatever that might be (e.g. an RDMS or MongoDB)
	/// </summary>
	public class RelationshipType : IDataStoreEntity
	{

        private Guid _objectId;

        /// <summary>
        /// Gets or sets the relationships's unique ID.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public int id { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public string relTypeText { get; set; }


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
