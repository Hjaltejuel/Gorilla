﻿using Entities.RedditEntities;
using System.Threading.Tasks;
using System;
using System.Net;
using Windows.UI.Xaml;
using System.Windows.Input;
using Entities.GorillaEntities;
using UITEST.Misc;
using UITEST.Model;
using UITEST.Model.GorillaRestInterfaces;
using UITEST.Model.RedditRestInterfaces;
using Post = Entities.RedditEntities.Post;

namespace UITEST.ViewModel
{
    public class PostPageViewModel : CommentableViewModel
    {
        public delegate void Comments();
        public event Comments CommentsReadyEvent;
        readonly IRedditApiConsumer _redditApiConsumer;
        readonly IRestUserPreferenceRepository _restUserPreferenceRepository;
        readonly IRestPostRepository _repository;
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

        public PostPageViewModel(INavigationService service, IRestPostRepository repository, IRestUserPreferenceRepository restUserPreferenceRepository, IRedditApiConsumer redditApiConsumer) 
            : base(service,repository,restUserPreferenceRepository,redditApiConsumer)
        {
            _redditApiConsumer = redditApiConsumer;
            _repository = repository;
            _restUserPreferenceRepository = restUserPreferenceRepository;
            PostLiked = new RelayCommand(async o => { await PostLikedAsync(); });
            PostDisliked = new RelayCommand(async o => { await PostDislikedAsync(); });
        }
        public async void GetCurrentPost(Post post)
        {
            CurrentPost = (await _redditApiConsumer.GetPostAndCommentsByIdAsync(post.id)).Item2;
            await _repository.CreateAsync(new Entities.GorillaEntities.Post { Id = post.id, username = UserFactory.GetInfo().name });
            CommentsReadyEvent?.Invoke();
        }

        public void Initialize(Post post)
        {
            CurrentPost = post;
            Votes = CurrentPost.score;
            GetCurrentPost(post);
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
            LikeCommentableAsync(_currentComment, direction);
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
            LikeCommentableAsync(_currentComment, direction);
        }
    }
}