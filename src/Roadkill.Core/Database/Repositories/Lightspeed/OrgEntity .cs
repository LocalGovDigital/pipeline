using System;
using Mindscape.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{
	[Table("pipeline_orgs")]
	[Cached(ExpiryMinutes = 1)]
	public class OrgEntity : Entity<int>
	{
		[Column("id")]
		private int _orgID;

		[Column("OrgName")]
		private string _orgname;

        [Column("email")]
        private string _email;

        [Column("url")]
        private string _url;

        [Column("twitter")]
        private string _twitter;

		
		public int Id
        {
            get
            {
                return _orgID;
            }
            set
            {
                Set<int>(ref _orgID, value);
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
