using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    public class Subreddit
    {
       
        [Key]
        [StringLength(100)]
        public string SubredditName { get; set; }

        [NotMapped]
        public virtual ICollection<SubredditConnection> SubredditFromConnections { get; set; }
        [NotMapped]
        public virtual ICollection<SubredditConnection> SubredditToConnections { get; set; }


    }
}
