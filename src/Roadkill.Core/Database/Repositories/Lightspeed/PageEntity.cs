using System;
using Mindscape.LightSpeed;
using Roadkill.Core.Database.Repositories.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{
    [Table("roadkill_pages", IdentityMethod = IdentityMethod.IdentityColumn)]
    public class PageEntity : BasePageEntity<int>
	{
        [Column("title")]
        private string _title;

        [Column("createdby")]
        private string _createdBy;

        [Column("createdon")]
        private DateTime _createdOnColumn;

        [Column("modifiedby")]
        private string _modifiedBy;

        [Column("modifiedon")]
        private DateTime _modifiedOn;

        [Column("projectstart")]
        private DateTime _projectstart;

        [Column("projectend")]
        private DateTime _projectEnd;

        [Column("projectEstimatedTime")]
        private bool _projectEstimatedTime;

        [Column("projectLanguage")]
        private string _projectLanguage;

        [Column("projectStatus")]
        private string _projectStatus;

        [Column("orgID")]
        private int _orgID;

        [Column("tags")]
        private string _tags;

        [Column("islocked")]
        private bool _isLocked;

        [ReverseAssociation("PageContents")]
        private readonly EntityCollection<PageContentEntity> _pageContents = new EntityCollection<PageContentEntity>();

        public EntityCollection<PageContentEntity> PageContents
        {
            get { return Get(_pageContents); }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                Set<string>(ref _title, value);
            }
        }

        public string CreatedBy
        {
            get
            {
                return _createdBy;
            }
            set
            {
                Set<string>(ref _createdBy, value);
            }
        }

        public DateTime CreatedOn
        {
            get
            {
                return _createdOnColumn;
            }
            set
            {
                Set<DateTime>(ref _createdOnColumn, value);
            }
        }

        public string ModifiedBy
        {
            get
            {
                return _modifiedBy;
            }
            set
            {
                Set<string>(ref _modifiedBy, value);
            }
        }

        public DateTime ModifiedOn
        {
            get
            {
                return _modifiedOn;
            }
            set
            {
                Set<DateTime>(ref _modifiedOn, value);
            }
        }

        public string Tags
        {
            get
            {
                return _tags;
            }
            set
            {
                Set<string>(ref _tags, value);
            }
        }

        public bool IsLocked
        {
            get
            {
                return _isLocked;
            }
            set
            {
                Set<bool>(ref _isLocked, value);
            }
        }

        public DateTime ProjectStart
        {
            get
            {
                return _projectstart;
            }
            set
            {
                Set<DateTime>(ref _projectstart, value);
            }
        }

        public DateTime ProjectEnd
        {
            get
            {
                return _projectEnd;
            }
            set
            {
                Set<DateTime>(ref _projectEnd, value);
            }
        }

        public bool ProjectEstimatedTime
        {
            get
            {
                return _projectEstimatedTime;
            }
            set
            {
                Set<bool>(ref _projectEstimatedTime, value);
            }
        }

        public string ProjectLanguage
        {
            get
            {
                return _projectLanguage;
            }
            set
            {
                Set<string>(ref _projectLanguage, value);
            }
        }


        public string ProjectStatus
        {
            get
            {
                return _projectStatus;
            }
            set
            {
                Set<string>(ref _projectStatus, value);
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
    }
}
