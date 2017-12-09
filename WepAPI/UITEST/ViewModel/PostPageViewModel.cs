using Entities.RedditEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UITEST.Model;

namespace UITEST.ViewModel
{
    public class PostPageViewModel : BaseViewModel
    {
        public delegate void CommentsReady();
        public event CommentsReady CommentsReadyEvent;
        private Post currentpost;

        public Post CurrentPost
        {
            get { return currentpost; }
            set {
                currentpost = value;
                OnPropertyChanged("CurrentPost");
            }
        }

        public AbstractCommentable FocusedAbstractCommentable { get; set; }

        public PostPageViewModel()
        {
        }
        public async void GetCurrentPost(Post post)
        {
            IRedditAPIConsumer redditAPIConsumer = new RedditConsumerController();
            CurrentPost = await redditAPIConsumer.GetPostAndCommentsByIdAsync(post.id);
            CommentsReadyEvent.Invoke();
        }
        public void Initialize(Post post)
        {
            GetCurrentPost(post);
        }
    }
}
