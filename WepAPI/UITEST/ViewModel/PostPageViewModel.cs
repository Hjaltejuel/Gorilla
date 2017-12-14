using Entities.RedditEntities;
using Gorilla.Model;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace UITEST.ViewModel
{
    public class PostPageViewModel : BaseViewModel
    {
        public delegate void Comments();
        public event Comments CommentsReadyEvent;
        private Post currentpost;
        IRedditAPIConsumer redditAPIConsumer;
        private bool IsLiked;
        private bool IsDisliked;
        private int _votes;
        public int votes
        {
            get { return _votes; }
            set { _votes = value; OnPropertyChanged(); }
        }

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
            redditAPIConsumer = App.ServiceProvider.GetService<IRedditAPIConsumer>();
            CurrentPost = post;
            votes = CurrentPost.score;
            GetCurrentPost(post);
        }

        public async Task AddCommentAsync(AbstractCommentable commentableToCommentOn, Comment newComment)
        {
            await redditAPIConsumer.CreateCommentAsync(commentableToCommentOn, newComment.body);
        }

        public delegate void Vote();
        public event Vote Like;
        public event Vote Dislike;


        //TODO hvis vi ikke kan få observer pattern til at virke kan vi slette de der currentcomment.score - og + statements
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
            await redditAPIConsumer.VoteAsync(currentpost, direction);
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
            await redditAPIConsumer.VoteAsync(currentpost, direction);
            Dislike.Invoke();
        }
    }
}