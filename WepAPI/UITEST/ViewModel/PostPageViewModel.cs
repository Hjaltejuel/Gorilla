using Entities.RedditEntities;
using Gorilla.Model;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace UITEST.ViewModel
{
    public class PostPageViewModel : BaseViewModel
    {
        public delegate void Comments();
        public event Comments CommentsReadyEvent;
        IRedditAPIConsumer redditAPIConsumer;
        private bool IsLiked;
        private bool IsDisliked;

        private int _votes;
        public int votes
        {
            get { return _votes; } set { _votes = value; OnPropertyChanged(); }
        }
        private Post _currentComment;
        public Post CurrentPost
        {
            get { return _currentComment; } set { _currentComment = value; OnPropertyChanged();  }
        }
        private string _timeSinceCreation;

        public string timeSinceCreation
        {
            get { return _timeSinceCreation; } set { _timeSinceCreation = value; OnPropertyChanged(); }
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
            redditAPIConsumer = App.ServiceProvider.GetService<IRedditAPIConsumer>();
            CurrentPost = post;
            votes = CurrentPost.score;
            GetCurrentPost(post);
            timeSinceCreation = TimeHelper.CalcCreationDateByUser(CurrentPost);
        }

        public async Task<Comment> AddCommentAsync(AbstractCommentable commentableToCommentOn, string newCommentText)
        {
            var old = new DateTime(1970, 1, 1);
            var totaltime = DateTime.Now - old;
            int timeInSeconds = (int)totaltime.TotalSeconds;
            var newComment = new Comment()
            {
                body = newCommentText,
                author = "ASD",
                created_utc = timeInSeconds
            };
            await redditAPIConsumer.CreateCommentAsync(commentableToCommentOn, newComment.body);
            return newComment;
        }

        public delegate void Vote();
        public event Vote Like;
        public event Vote Dislike;

        public async Task PostLikedAsync()
        {
            int direction;

            if (IsLiked)
            {
                votes -= 1;
                direction = 0;
            }
            else
            {
                if (IsDisliked)
                    votes += 2;
                else
                    votes += 1;
                direction = 1;
            }
            IsDisliked = false;
            IsLiked = !IsLiked;
            await redditAPIConsumer.VoteAsync(_currentComment, direction);
            Like.Invoke();
        }

        public async Task PostDislikedAsync()
        {
            int direction;

            if (IsDisliked)
            {
                votes += 1;
                direction = 0;
            }
            else
            {
                if (IsLiked)
                    votes -= 2;
                else
                    votes -= 1;
                direction = -1;
            }
            IsLiked = false;
            IsDisliked = !IsDisliked;
            await redditAPIConsumer.VoteAsync(_currentComment, direction);
            Dislike.Invoke();
        }
    }
}