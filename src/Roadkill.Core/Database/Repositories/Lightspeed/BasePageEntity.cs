using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mindscape.LightSpeed;

namespace Roadkill.Core.Database.Repositories.LightSpeed
{
    public class BasePageEntity<T> : Entity<T>
    {
        
        [Column("projectAgileLifeCyclePhase")]
        protected string _projectAgileLifeCyclePhase;

        [Column("department")]
        protected string _department;

        [Column("owner")]
        protected string _owner;

        [Column("ownerEmail")]
        protected string _ownerEmail;

        [Column("collaborationLevel")]
        protected string _collaborationLevel;

        [Column("nextserviceassessmentdate")]
        protected DateTime? _nextserviceassessmentdate;

        [Column("fundingboundary")]
        protected string _fundingBoundary;


        public string FundingBoundary
        {
            get
            {
                return _fundingBoundary;
            }
            set
            {
                Set<string>(ref _fundingBoundary, value);
            }
        }

        public string ProjectAgileLifeCyclePhase
        {
            get
            {
                return _projectAgileLifeCyclePhase;
            }
            set
            {
                Set<string>(ref _projectAgileLifeCyclePhase, value);
            }
        }

        public string Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                Set<string>(ref _owner, value);
            }
        }
        public string OwnerEmail
        {
            get
            {
                return _ownerEmail;
            }
            set
            {
                Set<string>(ref _ownerEmail, value);
            }
        }
        public string CollaborationLevel
        {
            get
            {
                return _collaborationLevel;
            }
            set
            {
                Set<string>(ref _collaborationLevel, value);
            }
        }   
        public DateTime? NextServiceAssessmentDate
        {
            get
            {
                return _nextserviceassessmentdate;
            }
            set
            {
                Set<DateTime?>(ref _nextserviceassessmentdate, value);
            }
        }
        public string Department
        {
            get
            {
                return _department;
            }
            set
            {
                Set<string>(ref _department, value);
            }
        }
    }
}
