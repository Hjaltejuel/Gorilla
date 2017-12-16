using System.ComponentModel.DataAnnotations;

namespace Entities.GorillaEntities
{
    public class Subreddit
    {
       
        [Key]
        [StringLength(100)]
        public string SubredditName { get; set; }

       


    }
}
