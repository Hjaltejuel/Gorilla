using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    public class SubredditConnection : IComparable<SubredditConnection>
    {
        [Key, ForeignKey("Subreddit")]
        [StringLength(100)]    
        public string SubredditFromName { get; set; }

        [Key, ForeignKey("Subreddit")]
        [StringLength(100)]
        public string SubredditToName { get; set; }
        

        public string Similarity { get; set; }


        public int CompareTo(SubredditConnection other)
        {
            if (Decimal.Parse(Similarity) > Decimal.Parse(other.Similarity)) { return -1; }
            if (Decimal.Parse(Similarity) == Decimal.Parse(other.Similarity)) { return 0; }
            return 1;
            
        }
    }
}
