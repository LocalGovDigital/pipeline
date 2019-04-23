using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Mvc.ViewModels
{
  public  class AddOrganisationRequestModel
    {
        [Required]
        public string Organisation { get; set; }
    }
}
