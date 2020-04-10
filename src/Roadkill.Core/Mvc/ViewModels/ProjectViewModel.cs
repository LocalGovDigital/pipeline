using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Roadkill.Core.Localization;
using Roadkill.Core.Database;
using Roadkill.Core.Text;
using Roadkill.Core.Converters;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using Roadkill.Core.Configuration;
using System.Web.Mvc;
using Roadkill.Core.Database.LightSpeed;
using Roadkill.Core.Database.Repositories.LightSpeed;
using Roadkill.Core.Hackney.Models;
using StructureMap;


namespace Roadkill.Core.Mvc.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? ProjectStart { get; set; }
        public DateTime? ProjectEnd { get; set; }
        public DateTime? NextServiceAssessmentDate { get; set; }
        public bool ProjectEstimatedTime { get; set; }
        public string ProjectStatus { get; set; }
        public string Title { get; set; }
        public string ModifiedOnWithOffset { get; set; }
        public string OrganisationName { get; set; }
        public string Department { get; set; }
    //    public string Owner { get; set; }
        public string OwnerEmail { get; set; }
        public string CollaborationLevel { get; set; }

        public string FundingBoundaryText { get; set; }

        public static ProjectViewModel Create(PageViewModel model)
        {
            var project = new ProjectViewModel();

            project.Id = model.Id;
            project.Content = model.Content;
            project.CreatedBy = model.CreatedBy;
            project.CreatedOn = model.CreatedOn;
            project.ModifiedBy = model.ModifiedBy;
            project.ModifiedOn = model.ModifiedOn;
            project.ProjectStart = model.ProjectStart;
            project.ProjectEnd = model.ProjectEnd;
            project.ProjectEstimatedTime = model.ProjectEstimatedTime;
            project.ProjectStatus = model.ProjectStatus;
            project.ModifiedOnWithOffset = model.ModifiedOnWithOffset;
            project.OrganisationName = model.OrgContactDetails?.OrgName;
            project.Department = model.Department;
            project.OwnerEmail = model.OwnerEmail;
            project.CollaborationLevel = model.CollaborationLevel;
            project.NextServiceAssessmentDate = model.NextServiceAssessmentDate;
            project.FundingBoundaryText = model.FundingBoundaryText;
            return project;
        }
    }
}
