using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roadkill.Core.Database;
using Roadkill.Core.Database.LightSpeed;

namespace Roadkill.Core.Models
{
    public class ProjectSearchResult
    {

        public string Title { get; set; }
        public string ProjectStatus { get; set; }
        public int OrganisationId { get; set; }
        public string Department { get; set; }
        public DateTime LastUpdated { get; set; }
        public string AgileLifecycle { get; set; }
        public string FundingBoundary { get; set; }
        public int Id { get; set; }
        public string Organisation { get; set; }

        public static ProjectSearchResult FromPageEntity(PageEntity entity)
        {
            var sr = new ProjectSearchResult
            {
                Title = entity.Title,
                Id = entity.Id,
                ProjectStatus = entity.ProjectStatus,
                AgileLifecycle = entity.ProjectAgileLifeCyclePhase,
                FundingBoundary = entity.FundingBoundary,
                OrganisationId = entity.orgID,
                Department = entity.Department,
                LastUpdated = entity.ModifiedOn
            };
            return sr;


        }

    }
}
