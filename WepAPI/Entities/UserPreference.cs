using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{

    public class UserPreference
    {
        [Key]
        [StringLength(50)]
        public string Username { get; set; }

        [Key]
        [StringLength(100)]
        public string SubredditName { get; set; }

        [ForeignKey("Username")]
        [InverseProperty("SubredditConnections")]
        [NotMapped]
        public virtual User User { get; set; }

        [ForeignKey("SubredditName")]
        [NotMapped]
        public virtual Subreddit Subreddit { get; set; }

        public Decimal PriorityMultiplier { get; set; }
    }
}
