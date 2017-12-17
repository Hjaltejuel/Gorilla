using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.GorillaEntities
{

    public class UserPreference
    {
        [Key]
        [StringLength(50)]
        public string Username { get; set; }

        [Key]
        [StringLength(100)]
        public string SubredditName { get; set; }

        public Decimal PriorityMultiplier { get; set; }
    }
}
