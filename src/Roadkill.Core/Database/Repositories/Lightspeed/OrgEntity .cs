using System;
using Mindscape.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{
	[Table("pipeline_orgs")]
	[Cached(ExpiryMinutes = 1)]
	public class OrgEntity : Entity<int>
	{
		[Column("id")]
		private int _orgid;

		[Column("OrgName")]
		private string _orgname;

		
		public int Id
        {
            get
            {
                return _orgid;
            }
            set
            {
                Set<int>(ref _orgid, value);
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

	}
}
