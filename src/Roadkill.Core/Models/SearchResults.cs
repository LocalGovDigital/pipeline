using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Models
{
    public class SearchResults<TResp>
    {
        public TResp Result { get; set; }

        public int Count { get; set; }



        public IList<string> Organisations { get; set; }
        public IList<string> Phases { get; set; }
        public IList<string> AgileLifeCycleItems { get; set; }

        public List<string> CollaborationLevels
        {
            get { return new List<string>() { "Share Ideas", "Open To Conversation", "Share Funding" }; }
        }

    }
}
