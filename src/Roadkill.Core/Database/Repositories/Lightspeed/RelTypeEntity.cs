using System;
using Mindscape.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{
	[Table("pipeline_relationship_types")]
	[Cached(ExpiryMinutes = 1)]
	public class RelTypeEntity : Entity<int>
	{
		[Column("id")]
		private int _id;

		[Column("reltypetext")]
		private string _relTypeTextid;

		
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

       
        public string relTypeText
        {
            get
            {
                return _relTypeTextid;
            }
            set
            {
                Set<string>(ref _relTypeTextid, value);
            }
        }
	}
}
