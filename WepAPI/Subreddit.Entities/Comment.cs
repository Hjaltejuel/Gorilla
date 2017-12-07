using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Subreddit.Entities
{
    public class Comment : ICommentable
    {
        public string subreddit_id { get; set; }
        public string Api_type{ get; set; }
        public string parent_id { get; set; }
        public string body { get; set; }
        public string modhash { get; set; }
        public ObservableCollection<Comment> children { get; set; }
    }
}
