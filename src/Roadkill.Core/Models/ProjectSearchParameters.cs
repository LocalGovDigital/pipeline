using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;

namespace Roadkill.Core.Models
{
    public class ProjectSearchParameters
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime? LastUpdateStart { get; set; }
        public DateTime? LastUpdateEnd { get; set; }
        public string Organisation { get; set; }
        public string Department { get; set; }
        public string Phase { get; set; }
        public string AgileLifecycle { get; set; }
        public string FundingBoundary { get; set; }
        public string CollaborationLevel { get; set; }


        public int? Take { get; set; }
        public int? Skip { get; set; }

        public string OrderBy = "Title";

        private string key = Guid.NewGuid().ToString();

        public override string ToString()
        {
            return key;
        }

        public static ProjectSearchParameters FromQuery(NameValueCollection nvc)
        {
            var s = new ProjectSearchParameters();

            if (nvc.Count > 1)
            {
                s.key = string.Empty;
                foreach (string key in nvc)
                {
                    s.key += $"{nvc[key]}.";
                }
            }

            if (nvc.AllKeys.Contains("title")) s.Title = nvc["title"];
            if (nvc.AllKeys.Contains("keyword")) s.Text = nvc["keyword"];
            if (nvc.AllKeys.Contains("lastupdatestart")) s.LastUpdateStart = Convert.ToDateTime(nvc["lastupdatestart"]);
            if (nvc.AllKeys.Contains("lastupdateend")) s.LastUpdateEnd = Convert.ToDateTime(nvc["lastupdateend"]);
            if (nvc.AllKeys.Contains("organisation")) s.Organisation = nvc["organisation"];
            if (nvc.AllKeys.Contains("department")) s.Department = nvc["department"];
            if (nvc.AllKeys.Contains("phase")) s.Phase = nvc["phase"];
            if (nvc.AllKeys.Contains("agileLifecycle")) s.AgileLifecycle = nvc["agileLifecycle"];
            if (nvc.AllKeys.Contains("fundingboundary")) s.AgileLifecycle = nvc["fundingboundary"];
            if (nvc.AllKeys.Contains("collaborationLevel")) s.CollaborationLevel = nvc["collaborationLevel"];


            if (nvc.AllKeys.Contains("orderBy")) s.OrderBy = nvc["orderBy"];

            if (nvc.AllKeys.Contains("take")) s.Take = Convert.ToInt32(nvc["take"]);
            if (nvc.AllKeys.Contains("skip")) s.Skip = Convert.ToInt32(nvc["skip"]);
            return s;
        }
    }
}
