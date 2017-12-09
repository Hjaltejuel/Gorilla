using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.RedditEntities
{
    public class morechildren
    {
        public string api_type { get; set; }
        public string Children { get; set; } //list assorted by commas
        public string id { get; set; }//id of assosiated morechildren
        public bool limitchildren { get; set; }
        public string sort { get; set; } 
        //one of(confidence, top, new, controversial, old, random, qa, live)
    }
}
