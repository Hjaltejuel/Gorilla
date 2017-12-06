using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
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
