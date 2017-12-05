using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    public class CategorySubreddit
    {
        [Key]
        [StringLength(50)]
        public string Name { get; set; }

        [ForeignKey("Subreddit")]
        [StringLength(100)]
        public string SubredditName { get; set; }

        
        public virtual Subreddit Subreddit { get; set; }
    }
}
