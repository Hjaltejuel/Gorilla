using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Subreddit.Entities
{
    public class Vote
    {
        public int votedir { get; set; } ///-1,0,+1
        public string id { get; set; }
        public int rank { get; set; } //greater than 1
        public string modhash { get; set; }
    }
}
