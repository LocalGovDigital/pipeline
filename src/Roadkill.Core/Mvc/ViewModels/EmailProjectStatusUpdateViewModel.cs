using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Mvc.ViewModels
{
    public class EmailProjectStatusUpdateViewModel : EmailViewModel
    {
        public EmailProjectStatusUpdateViewModel()
        {
            Name=string.Empty;
            ProjectName = string.Empty;
            ProjectId = string.Empty;
            AdditionalMessage= string.Empty;
            UpdateDate = string.Empty;
            StatusUpdate = string.Empty;
            Author = string.Empty;
        }
        public string Name { get; set; }
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }
        public string AdditionalMessage { get; set; }
        public string UpdateDate { get; set; }
        public string StatusUpdate { get; set; }
        public string Author { get; set; }
    }
}
