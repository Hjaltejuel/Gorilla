using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.GorillaEntities
{
    public class CategorySubreddit
    {
        [Key]
        [StringLength(50)]
        public string Name { get; set; }

        [Key]
        [ForeignKey("Subreddit")]
        [StringLength(100)]
        public string SubredditName { get; set; }
    }
}
