using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roadkill.Core.Database.Repositories.LightSpeed;

namespace Roadkill.Core.Mvc.ViewModels
{
    public class StatusUpdateViewModel
    {

        public int Id
        {
            get; set;
        }
        public int PageId
        {
            get; set;
        }

        public DateTime UpdateDate
        {
            get; set;
        }
        public string Text { get; set; }
        public string Author
        {
            get; set;
        }
        public static StatusUpdateViewModel FromModel(StatusUpdate model)
        {
            var statusUpdate = new StatusUpdateViewModel();

            statusUpdate.Text = model.Text;
            statusUpdate.PageId = model.PageId;
            statusUpdate.Id = model.Id;
            statusUpdate.Author = model.Author;
            statusUpdate.UpdateDate = model.UpdateDate;
            return statusUpdate;
        }

    }
}
