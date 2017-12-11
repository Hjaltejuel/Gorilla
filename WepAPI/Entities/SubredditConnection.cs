using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    public class SubredditConnection
    {
        [Key, ForeignKey("Subreddit")]
        [StringLength(100)]    
        public string SubredditFromName { get; set; }

        [Key, ForeignKey("Subreddit")]
        [StringLength(100)]
        public string SubredditToName { get; set; }
        

        public decimal PPMI { get; set; }

        public int Count { get; set; }
    }
}
