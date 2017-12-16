using System.ComponentModel.DataAnnotations;

namespace Entities.GorillaEntities
{
    public class Post
    {
        [Key]
        public string Id { get; set; }

        [Key]
        public string username { get; set; }
    }
}
