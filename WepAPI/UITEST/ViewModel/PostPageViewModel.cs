using Entities.RedditEntities;
using Gorilla.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UITEST.Model;
using UITEST.View;

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


        

        public PostPageViewModel(INavigationService service) :base(service)
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
            CurrentPost = post;
            GetCurrentPost(post);
        }

        public void AddComment(AbstractCommentable commentableToCommentOn, Comment newComment)
        {

        }
    }
}
