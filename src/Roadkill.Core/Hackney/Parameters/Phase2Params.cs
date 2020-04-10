using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roadkill.Core.Database;
using Roadkill.Core.Hackney.Models;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Hackney.Parameters
{
    public class Phase2Params : Phase2PageModel
    {
        public static Phase2Params FromModel(PageViewModel model)
        {
            var parameters = new Phase2Params();
            parameters.Department = model.Department;
            parameters.Owner = model.Owner;
            parameters.OwnerEmail = model.OwnerEmail;
            parameters.CollaborationLevel = model.CollaborationLevel;
            parameters.NextServiceAssessmentDate = model.NextServiceAssessmentDate;
            parameters.ProjectAgileLifeCyclePhase = model.ProjectAgileLifeCyclePhase;
            parameters.FundingBoundary = model.FundingBoundary;

            return parameters;
        }

        public static Phase2Params FromPageContent(PageContent model)
        {
            var parameters = new Phase2Params();
            parameters.Department = model.Department;
            parameters.Owner = model.Owner;
            parameters.OwnerEmail = model.OwnerEmail;
            parameters.CollaborationLevel = model.CollaborationLevel;
            parameters.NextServiceAssessmentDate = model.NextServiceAssessmentDate;
            parameters.ProjectAgileLifeCyclePhase = model.ProjectAgileLifeCyclePhase;
            parameters.FundingBoundary = model.FundingBoundary;
            return parameters;
        }

        public static Phase2Params Empty()
        {
            var parameters = new Phase2Params();
            parameters.Department = String.Empty;
            parameters.Owner = String.Empty;
            parameters.OwnerEmail = String.Empty;
            parameters.CollaborationLevel = string.Empty;
            parameters.ProjectAgileLifeCyclePhase = String.Empty;
            parameters.FundingBoundary = String.Empty;
            return parameters;
        }

        public static Phase2Params Create(string projectAgileLifeCyclePhase, string department, string owner, string ownerEmail, string collaborationLevel, DateTime nextServiceAssessmentDate, string fundingBoundary)
        {
            var parameters = new Phase2Params();
            parameters.Department = department;
            parameters.Owner = owner;
            parameters.OwnerEmail = ownerEmail;
            parameters.CollaborationLevel = collaborationLevel;
            parameters.NextServiceAssessmentDate = nextServiceAssessmentDate;
            parameters.ProjectAgileLifeCyclePhase = projectAgileLifeCyclePhase;
            parameters.FundingBoundary = fundingBoundary;
            return parameters;
        }
    }
}
