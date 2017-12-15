using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class Post
    {
        [Key]
        public string Id { get; set; }

        [Key]
        public string username { get; set; }
    }
}
