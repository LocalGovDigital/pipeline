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

        [Column("userid")]
        private Guid _userid;
        
        [Column("orgID")]
        private int _orgID;

        [Column("pageid")]
        private int _pageid;

        [Column("reltypeid")]
        private int _reltypeid;

        [Column("reltext")]
        private string _reltext;

        [Column("reldatetime")]
        private DateTime _reldatetime;

        [Column("approved")]
        private bool _approved;
        [Column("pending")]
        private bool _pending;

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
        public bool Approved
        {
            get
            {
                return _approved;
            }
            set
            {
                Set<bool>(ref _approved, value);
            }
        }
        public bool Pending
        {
            get
            {
                return _pending;
            }
            set
            {
                Set<bool>(ref _pending, value);
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
        public Guid UserId
        {
            get
            {
                return _userid;
            }
            set
            {
                Set<Guid>(ref _userid, value);
            }
        }

        public int orgID
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
