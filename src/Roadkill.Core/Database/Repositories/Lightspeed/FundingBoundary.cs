using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mindscape.LightSpeed;

namespace Roadkill.Core.Database.Repositories.LightSpeed
{
   // [Table("pipeline_fundingboundary")]
   // [Cached(ExpiryMinutes = 1)]
    public class FundingBoundary //: Entity<string>
    {

        [Column("id")]
        private string _id;

        [Column("Text")]
        private string _text;

        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;

                //   Set<string>(ref _id, value);
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
                _text = value;
               // Set<string>(ref _text, value);
            }
        }
    }
}
