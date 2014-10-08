using System;
using Mindscape.LightSpeed;


namespace Roadkill.Core.Database.LightSpeed
{
    [Table("pipeline_relationship", IdentityMethod = IdentityMethod.IdentityColumn)]
    [Cached(ExpiryMinutes = 1)]
    public class RelEntity : Entity<int>
    {
        [Column("id")]
        private int _id;

        [Column("username")]
        private string _username;

        [Column("orgid")]
        private int _orgid;

        [Column("pageid")]
        private int _pageid;

        [Column("reltypeid")]
        private int _reltypeid;

        [Column("reltext")]
        private string _reltext;

        [Column("reldatetime")]
        private DateTime _reldatetime; 

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

        public string username
        {
            get
            {
                return _username;
            }
            set
            {
                Set<string>(ref _username, value);
            }
        }

        public int orgId
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

        public int pageId
        {
            get
            {
                return _pageid;
            }
            set
            {
                Set<int>(ref _pageid, value);
            }
        }

        public int relTypeId
        {
            get
            {
                return _reltypeid;
            }
            set
            {
                Set<int>(ref _reltypeid, value);
            }
        }

        public string relText
        {
            get
            {
                return _reltext;
            }
            set
            {
                Set<string>(ref _reltext, value);
            }
        }

        public DateTime relDateTime
        {
            get
            {
                return _reldatetime;
            }
            set
            {
                Set<DateTime>(ref _reldatetime, value);
            }
        }




    }
}
