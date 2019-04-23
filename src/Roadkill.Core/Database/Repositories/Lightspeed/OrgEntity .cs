using System;
using Mindscape.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{
	[Table("pipeline_orgs", IdentityMethod = IdentityMethod.IdentityColumn)]
	public class OrgEntity : Entity<int>
	{
		[Column("id")]
		private int _id;

		[Column("OrgName")]
		private string _orgname;

        [Column("email")]
        private string _email;

        [Column("url")]
        private string _url;

        [Column("twitter")]
        private string _twitter;

		
		public int id
        {
            get
            {
                return _id;
            }
            set
            {
                Set<int>(ref _id, value);
            }
        }

		internal string OrgName
		{
			get
			{
                return _orgname;
			}
			set
			{
                Set<string>(ref _orgname, value);
			}
		}

        internal string email
        {
            get
            {
                return _email;
            }
            set
            {
                Set<string>(ref _email, value);
            }
        }

        internal string url
        {
            get
            {
                return _url;
            }
            set
            {
                Set<string>(ref _url, value);
            }
        }

        internal string twitter
        {
            get
            {
                return _twitter;
            }
            set
            {
                Set<string>(ref _twitter, value);
            }
        }

	}
}
