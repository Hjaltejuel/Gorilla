using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.RedditEntities
{
    public class Report
    {
        public string Api_type { get; set; }
        public string OtherReason { get; set; } //a string not longer than 100 chars
        public string Reason { get; set; } //a string not longer than 100 chars
        public string Rule_reason { get; set; } //a string not longer than 100 chars
        public string Site_reason { get; set; } //a string not longer than 100 chars
        public string Thing_id { get; set; }
        public string Modhash { get; set; }
    }
}
