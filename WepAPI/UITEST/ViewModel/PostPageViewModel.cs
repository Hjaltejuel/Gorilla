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
        IRedditAPIConsumer redditAPIConsumer;
        private bool IsLiked;
        private bool IsDisliked;

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
            CurrentPost = await redditAPIConsumer.GetPostAndCommentsByIdAsync(post.id);
            CommentsReadyEvent.Invoke();
        }

        public void Initialize(Post post)
        {
            redditAPIConsumer = new RedditConsumerController();
            CurrentPost = post;
            GetCurrentPost(post);
        }

        public void AddComment(AbstractCommentable commentableToCommentOn, Comment newComment)
        {
            redditAPIConsumer.CreateCommentAsync(commentableToCommentOn, newComment.body);
        }

        public delegate void Vote();
        public event Vote Like;
        public event Vote Dislike;

        public async Task PostLikedAsync()
        {
            int direction;

            if (IsLiked)
            {
                CurrentPost.score -= 1;
                direction = 0;
            }
            else
            {
                if (IsDisliked)
                    CurrentPost.score += 2;
                else 
                    CurrentPost.score += 1;
                direction = 1;
            }
            IsDisliked = false;
            IsLiked = !IsLiked;
            await redditAPIConsumer.VoteAsync(currentpost, direction);
            Like.Invoke();
        }

        public async Task PostDislikedAsync()
        {
            int direction;

            if (IsDisliked)
            {
                CurrentPost.score += 1;
                direction = 0;
            }
            else
            {
                if (IsLiked)
                    CurrentPost.score -= 2;
                else
                    CurrentPost.score -= 1;
                OnPropertyChanged("CurrentPost");
                direction = -1;
            }
            IsLiked = false;
            IsDisliked = !IsDisliked;
            await redditAPIConsumer.VoteAsync(currentpost, direction);
            Dislike.Invoke();
        }
    }
}