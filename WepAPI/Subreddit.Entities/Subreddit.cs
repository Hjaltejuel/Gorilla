using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Subreddit.Entities
{
    public class Subreddit : ICommentable
    {
        ObservableCollection<Post> posts;

        public Subreddit(ObservableCollection<Post> posts)
        {
            this.posts = posts;
        }
    }
}
