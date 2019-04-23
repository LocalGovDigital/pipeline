using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Mvc.ViewModels
{
    public class EmailProjectUpdateViewModel : EmailViewModel
    {
        public EmailProjectUpdateViewModel()
        {
            Name=string.Empty;
            ProjectName = string.Empty;
            ProjectId = string.Empty;
            AdditionalMessage= string.Empty;
        }
        public string Name { get; set; }
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }
        public string AdditionalMessage { get; set; }
    }
}
