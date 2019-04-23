using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Security.X509;

namespace Roadkill.Core.Mvc.ViewModels
{
    public abstract class EmailViewModel
    {

        public EmailViewModel()
        {

            ToAddress = string.Empty;
            CcAddress = string.Empty;
            BccAddress = string.Empty;
            Subject = string.Empty;
        }
        public string ToAddress { get; set; }
        public string CcAddress { get; set; }
        public string BccAddress { get; set; }
        public string Subject { get; set; }
    }
}
