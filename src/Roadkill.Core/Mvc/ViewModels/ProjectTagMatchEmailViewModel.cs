using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Mvc.ViewModels
{
    public class ProjectTagMatchEmailViewModel : EmailViewModel
    {
        public ProjectTagMatchEmailViewModel()
        {
            Name = string.Empty;
            ProjectName = string.Empty;
            ProjectId = string.Empty;
            Tags = string.Empty;
            NewProjectName = string.Empty;
            NewProjectId = string.Empty;
            AdditionalMessage = string.Empty;
            UpdateDate = string.Empty;
            Author = string.Empty;
        }
        public string Name { get; set; }
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }
        public string NewProjectName { get; set; }
        public string NewProjectId { get; set; }
        public string AdditionalMessage { get; set; }
        public string UpdateDate { get; set; }
        public string Tags { get; set; }
        public string Author { get; set; }
    }
}
