using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mindscape.LightSpeed;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Database.Repositories.LightSpeed
{
    [Table("pipeline_statusupdate", IdentityMethod = IdentityMethod.IdentityColumn)]
    [Cached(ExpiryMinutes = 1)]
    public class StatusUpdate : Entity<int>
    {

        [Column("id")]
        private int _id;

        [Column("pageid")]
        private int _pageid;

        [Column("author")]
        private string _author;

        [Column("text")]
        private string _text;

        [Column("UpdateDate")]
        private DateTime _updateDate;


        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                Set<int>(ref _id, value);
            }
        }
        public int PageId
        {
            get
            {
                return _pageid;
            }
            set
            {
                Set<int>(ref _pageid, value);
            }
        }

        public DateTime UpdateDate
        {
            get
            {
                return _updateDate;
            }
            set
            {
                Set<DateTime>(ref _updateDate, value);
            }
        }
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                Set<string>(ref _text, value);
            }
        }
        public string Author
        {
            get
            {
                return _author;
            }
            set
            {
                Set<string>(ref _author, value);
            }
        }

        public static StatusUpdate FromViewModel(StatusUpdateViewModel model)
        {
            var statusUpdate = new StatusUpdate();

            statusUpdate.Text = model.Text;
            statusUpdate.PageId = model.PageId;
            statusUpdate.Id = model.Id;
            statusUpdate.UpdateDate = model.UpdateDate;
            statusUpdate.Author = model.Author;
            return statusUpdate;
        }
    }
}
