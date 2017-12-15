using Gorilla.AuthenticationGorillaAPI;
using Gorilla.Model;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UITEST.RedditInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml;
using Gorilla.Model.GorillaRestInterfaces;
using Entities.RedditEntities;

namespace UITEST.ViewModel
{
    public class CommentableViewModel : BaseViewModel
    {
        IRedditAPIConsumer _redditAPIConsumer;
        IRestUserPreferenceRepository _restUserPreferenceRepository;
        IRestPostRepository _repository;
        private bool IsLiked;
        private bool IsDisliked;
        private Style _likeButton;
        public Style likeButton
        {
            get { return _likeButton; }
            set
            {
                _likeButton = value;
                OnPropertyChanged();
            }
        }
        private Style _dislikeButton;
        public Style dislikeButton
        {
            get { return _dislikeButton; }
            set { _dislikeButton = value; OnPropertyChanged(); }
        }
        private int _votes;
        public int votes
        {
            get { return _votes; }
            set { _votes = value; OnPropertyChanged(); }
        }
        private string _timeSinceCreation;
        public string timeSinceCreation
        {
            get { return _timeSinceCreation; }
            set { _timeSinceCreation = value; OnPropertyChanged(); }
        }
        private AbstractCommentable _currentCommentable;
        public AbstractCommentable CurrentCommentable
        {
            get { return _currentCommentable; }
            set { _currentCommentable = value; OnPropertyChanged(); }
        }
        public CommentableViewModel(INavigationService service, IRestPostRepository repository, IRestUserPreferenceRepository restUserPreferenceRepository, IRedditAPIConsumer redditAPIConsumer) : base(service)
        {
            _repository = repository;
            _restUserPreferenceRepository = restUserPreferenceRepository;
            _redditAPIConsumer = redditAPIConsumer;
        }
        public void Initialize(AbstractCommentable commentable)
        {
            CurrentCommentable = commentable;
            likeButton = App.Current.Resources["LikeButton"] as Style;
            dislikeButton = App.Current.Resources["DislikeButton"] as Style;
            votes = CurrentCommentable.score;
            timeSinceCreation = TimeHelper.CalcCreationDateByUser(CurrentCommentable);
        }

        public async Task CommentableLikedAsync()
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
            await _redditAPIConsumer.VoteAsync(CurrentCommentable, direction);
            await _restUserPreferenceRepository.UpdateAsync(new Entities.UserPreference { Username = UserFactory.GetInfo().name, SubredditName = CurrentCommentable.subreddit, PriorityMultiplier = 1 });
        }

        public async Task CommentableDislikedAsync()
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
            await _redditAPIConsumer.VoteAsync(CurrentCommentable, direction);
            await _restUserPreferenceRepository.UpdateAsync(new Entities.UserPreference { Username = UserFactory.GetInfo().name, SubredditName = CurrentCommentable.subreddit, PriorityMultiplier = 1 });
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
            await _redditAPIConsumer.CreateCommentAsync(commentableToCommentOn, newComment.body);
            await _restUserPreferenceRepository.UpdateAsync(new Entities.UserPreference { Username = UserFactory.GetInfo().name, SubredditName = CurrentCommentable.subreddit, PriorityMultiplier = 3 });
            return newComment;
        }

    }
}
