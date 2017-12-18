

using Entities.RedditEntities;
using System.Threading.Tasks;
using System;
using System.Net;
using Windows.UI.Xaml;
using System.Windows.Input;
using Entities.GorillaEntities;
using UI.Lib.Misc;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;
using Post = Entities.RedditEntities.Post;

namespace UI.Lib.ViewModel
{
    public class PostPageViewModel : CommentableViewModel
    {
        public delegate void Comments();
        public event Comments CommentsReadyEvent;
        readonly IRedditApiConsumer _redditApiConsumer;
        readonly IRestUserPreferenceRepository _restUserPreferenceRepository;
        readonly IRestPostRepository _repository;
        private readonly IUserHandler _userHandler;
        public ICommand PostLiked;
        public ICommand PostDisliked;
        private bool _isLiked;
        private bool _isDisliked;
        private Style _likeButton;
        public Style LikeButton { get => _likeButton;
            set { _likeButton = value; OnPropertyChanged(); } }
        private Style _dislikeButton;
        public Style DislikeButton { get => _dislikeButton;
            set { _dislikeButton = value; OnPropertyChanged(); } }
        private int _votes;
        public int Votes { get => _votes;
            set { _votes = value; OnPropertyChanged(); } }
        private Post _currentComment;
        public Post CurrentPost { get => _currentComment;
            set { _currentComment = value; OnPropertyChanged();  } }
        private string _timeSinceCreation;
        public string TimeSinceCreation { get => _timeSinceCreation;
            set { _timeSinceCreation = value; OnPropertyChanged(); }}

        public PostPageViewModel(INavigationService service, IRestPostRepository repository, IRestUserPreferenceRepository restUserPreferenceRepository, IRedditApiConsumer redditApiConsumer, IUserHandler userHandler) 
            : base(service,restUserPreferenceRepository,redditApiConsumer, userHandler)
        {
            _redditApiConsumer = redditApiConsumer;
            _repository = repository;
            _restUserPreferenceRepository = restUserPreferenceRepository;
            _userHandler = userHandler;
            PostLiked = new RelayCommand(async o => { await PostLikedAsync(); });
            PostDisliked = new RelayCommand(async o => { await PostDislikedAsync(); });
        }
        public async Task SetCurrentPost(Post post)
        {
            var redditResult = await _redditApiConsumer.GetPostAndCommentsByIdAsync(post.id);
            if (redditResult.Item1 == HttpStatusCode.OK) {
                CurrentPost = (redditResult).Item2;
                await _repository.CreateAsync(new Entities.GorillaEntities.Post
                {
                    Id = post.id,
                    username = _userHandler.GetUserName()
                });
                CommentsReadyEvent?.Invoke();
            }
        }

        public async Task Initialize(Post post)
        {
            CurrentPost = post;
            Votes = CurrentPost.score;
            await SetCurrentPost(post);
            TimeSinceCreation = TimeHelper.CalcCreationDateByUser(CurrentPost);
            LikeButton = Application.Current.Resources["LikeButton"] as Style;
            DislikeButton = Application.Current.Resources["DislikeButton"] as Style;
        }
        public async Task PostLikedAsync()
        {
            int direction;

            if (_isLiked)
            {
                Votes -= 1;
                direction = 0;
                LikeButton = Application.Current.Resources["LikeButton"] as Style;
            }
            else
            {
                if (_isDisliked)
                    Votes += 2;
                else
                    Votes += 1;
                direction = 1;
                LikeButton = Application.Current.Resources["LikeButtonClicked"] as Style;
            }
            _isDisliked = false;
            _isLiked = !_isLiked;
            DislikeButton = Application.Current.Resources["DislikeButton"] as Style;
            await LikeCommentableAsync(_currentComment, direction);
        }

        public async Task PostDislikedAsync()
        {
            int direction;

            if (_isDisliked)
            {
                Votes += 1;
                direction = 0;
                DislikeButton = Application.Current.Resources["DislikeButton"] as Style;
            }
            else
            {
                if (_isLiked)
                    Votes -= 2;
                else
                    Votes -= 1;
                direction = -1;
                DislikeButton = Application.Current.Resources["DislikeButtonClicked"] as Style;
            }
            _isLiked = false;
            _isDisliked = !_isDisliked;
            LikeButton = Application.Current.Resources["LikeButton"] as Style;
           await LikeCommentableAsync(_currentComment, direction);
        }
    }
}