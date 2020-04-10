using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Hackney.Models
{
    public abstract class Phase2PageModel
    {

        #region hackney-pipeline


        public string Department { get; set; }
        public string Owner { get; set; }
        public string OwnerEmail { get; set; }
        public string ProjectAgileLifeCyclePhase { get; set; }
        public string CollaborationLevel { get; set; }
        public DateTime? NextServiceAssessmentDate { get; set; }
        public string FundingBoundary { get; set; }
        public string FundingBoundaryText { get; set; }
        #endregion
    }
}
