using Entities.RedditEntities;
using Gorilla.Model;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;
using Windows.UI.Xaml;
using Gorilla.Model.GorillaRestInterfaces;
using UITEST.RedditInterfaces;
using Model;

namespace UITEST.ViewModel
{
    public class PostPageViewModel : BaseViewModel
    {
        public delegate void Comments();
        public event Comments CommentsReadyEvent;
        IRedditAPIConsumer redditAPIConsumer;
        IRestUserPreferenceRepository _restUserPreferenceRepository;
        IRestPostRepository _repository;
        private bool IsLiked;
        private bool IsDisliked;
        private Style _likeButton;
        public Style likeButton { get { return _likeButton; } set { _likeButton = value; OnPropertyChanged(); } }
        private Style _dislikeButton;
        public Style dislikeButton { get { return _dislikeButton; } set { _dislikeButton = value; OnPropertyChanged(); } }
        private int _votes;
        public int votes { get { return _votes; } set { _votes = value; OnPropertyChanged(); } }
        private Post _currentComment;
        public Post CurrentPost { get { return _currentComment; } set { _currentComment = value; OnPropertyChanged();  } }
        private string _timeSinceCreation;
        public string timeSinceCreation { get { return _timeSinceCreation; } set { _timeSinceCreation = value; OnPropertyChanged(); }}

        public PostPageViewModel(INavigationService service, IRestPostRepository repository, IRestUserPreferenceRepository restUserPreferenceRepository) : base(service)
        {
            _repository = repository;
            _restUserPreferenceRepository = restUserPreferenceRepository;


        }
        public async void GetCurrentPost(Post post)
        {
            CurrentPost = await redditAPIConsumer.GetPostAndCommentsByIdAsync(post.id);
            await _repository.CreateAsync(new Entities.Post { Id = post.id, username = UserFactory.GetInfo().name });

            CommentsReadyEvent.Invoke();
        }

        public void Initialize(Post post)
        {
            redditAPIConsumer = App.ServiceProvider.GetService<IRedditAPIConsumer>();
            CurrentPost = post;
            votes = CurrentPost.score;
            GetCurrentPost(post);
            timeSinceCreation = TimeHelper.CalcCreationDateByUser(CurrentPost);
            likeButton = App.Current.Resources["LikeButton"] as Style;
            dislikeButton = App.Current.Resources["DislikeButton"] as Style;
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
            await  _restUserPreferenceRepository.UpdateAsync(new Entities.UserPreference { Username = UserFactory.GetInfo().name, SubredditName = CurrentPost.subreddit, PriorityMultiplier = 3 });
            return newComment;
        }

        public async Task PostLikedAsync()
        {
          
            int direction;

            if (IsLiked)
            {
                votes -= 1;
                direction = 0;
                likeButton = App.Current.Resources["LikeButton"] as Style;
            }
            else
            {
                if (IsDisliked)
                    votes += 2;
                else
                    votes += 1;
                direction = 1;
                likeButton = App.Current.Resources["LikeButtonClicked"] as Style;
            }
            IsDisliked = false;
            IsLiked = !IsLiked;
            dislikeButton = App.Current.Resources["DislikeButton"] as Style;
            await redditAPIConsumer.VoteAsync(_currentComment, direction);
            await _restUserPreferenceRepository.UpdateAsync(new Entities.UserPreference { Username = UserFactory.GetInfo().name, SubredditName = CurrentPost.subreddit, PriorityMultiplier = 1 });
        }

        public async Task PostDislikedAsync()
        {
            
            int direction;

            if (IsDisliked)
            {
                votes += 1;
                direction = 0;
                dislikeButton = App.Current.Resources["DislikeButton"] as Style;
            }
            else
            {
                if (IsLiked)
                    votes -= 2;
                else
                    votes -= 1;
                direction = -1;
                dislikeButton = App.Current.Resources["DislikeButtonClicked"] as Style;
            }
            IsLiked = false;
            IsDisliked = !IsDisliked;
            likeButton = App.Current.Resources["LikeButton"] as Style;
            await redditAPIConsumer.VoteAsync(_currentComment, direction);
            await _restUserPreferenceRepository.UpdateAsync(new Entities.UserPreference { Username = UserFactory.GetInfo().name, SubredditName = CurrentPost.subreddit, PriorityMultiplier = 1 });
        }
    }
}