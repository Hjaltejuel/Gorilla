﻿using Entities.RedditEntities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;

namespace UI.Lib.ViewModel
{
    public class ProfilePageViewModel : BaseViewModel
    {
        private ObservableCollection<Post> _posts;

        public ObservableCollection<Post> Posts
        {
            get => _posts;
            set { _posts = value; OnPropertyChanged(); }
        }

        //ProfileInformation i region
        #region

        public delegate void PostsReady();
        public event PostsReady PostsReadyEvent;
        private string _username;
        public string Username { get => _username; set { if (value != _username) { _username = value; OnPropertyChanged(); } } }
        private int _amountOfSubRedditsSubscribedTo;
        public int AmountOfSubRedditsSubscribedTo { get => _amountOfSubRedditsSubscribedTo; set { if (value != _amountOfSubRedditsSubscribedTo) { _amountOfSubRedditsSubscribedTo = value; OnPropertyChanged(); } } }
        private DateTime _joinDate;
        public DateTime JoinDate { get => _joinDate; set { if (value != _joinDate) { _joinDate = value; OnPropertyChanged(); } } }
        private int _commentKarma;
        public int CommentKarma { get => _commentKarma; set { if (value != _commentKarma) { _commentKarma = value; OnPropertyChanged(); } } }
        private int _linkKarma;
        public int LinkKarma { get => _linkKarma; set { if (value != _linkKarma) { _linkKarma = value; OnPropertyChanged(); } } }
        private string _postsCreated;
        public string PostsCreated { get => _postsCreated; set { if (value != _postsCreated) { _postsCreated = value; OnPropertyChanged(); } } }
        private string _commentsCreated;
        public string CommentsCreated { get => _commentsCreated; set { if (value != _commentsCreated) { _commentsCreated = value; OnPropertyChanged(); } } }
        #endregion

        public ICommand GoToPostPageCommand { get; set; }
        private readonly IRestUserRepository _repository;
        private readonly IRestPostRepository _restPostRepository;
        private readonly IRedditApiConsumer _consumer;
        private readonly IUserHandler _userHandler;
        private ImageSource _image;
        public ImageSource Image
        {
            get => _image;
            set
            {
                if (_image != value)
                {
                    _image = value;
                    OnPropertyChanged();
                }
            }
        }

        private byte[] _imageBytes;
        public byte[] ImageBytes
        {
            get => _imageBytes;
            set
            {
                if (_imageBytes != value)
                {
                    _imageBytes = value;
                    OnPropertyChanged();
                    LoadImageAsync();
                }
            }
        }

        public ProfilePageViewModel(INavigationService service, IRestUserRepository repository, IRedditApiConsumer consumer, IRestPostRepository restPostRepository, IUserHandler userHandler) : base(service)
        {
            _restPostRepository = restPostRepository;
            _repository = repository;
            _consumer = consumer;
            _userHandler = userHandler;
        }

        public async Task Initialize()
        {
            await GetCurrentProfile();

            if (_userHandler.GetProfilePic() == null)
            {
                ImageBytes = await _repository.FindImageAsync(Username);
            } else
            {
                ImageBytes = _userHandler.GetProfilePic();
            }
            var visitedPosts = await _restPostRepository.ReadAsync(Username);
            var ids = visitedPosts.Aggregate("", (current, post) => "t3_"+current + ",t3_" + post.Id);
            Posts = (await _consumer.GetPostsByIdAsync(ids)).Item2;
            PostsReadyEvent?.Invoke();
        }

        private async Task GetCurrentProfile()
        {
            var redditUser = _userHandler.GetUser();
            var subscriptions = (await _consumer.GetSubscribedSubredditsAsync()).Item2;
            var userPosts = (await _consumer.GetUserPosts(redditUser.name)).Item2;
            var userComments = (await _consumer.GetUserComments(redditUser.name)).Item2;

            var numberOfPosts = userPosts.Count > 25 ? "25+" : userPosts.Count.ToString();
            var numberOfComments = userComments.Count > 25 ? "25+" : userComments.Count.ToString();
            var unix = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var time = unix.AddSeconds(redditUser.created);
            
            Username = redditUser.name;
            AmountOfSubRedditsSubscribedTo = subscriptions.Count;
            JoinDate = time;
            CommentKarma = redditUser.comment_karma;
            LinkKarma = redditUser.link_karma;
            PostsCreated = numberOfPosts;
            CommentsCreated = numberOfComments;
        }
        public async Task LoadImageAsync()
        {
            if (ImageBytes == null)
            {
                Image = null;
            }
            var image = new BitmapImage();
            using (var stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(ImageBytes.AsBuffer());
                stream.Seek(0);
                await image.SetSourceAsync(stream);
            }
            Image = image;
        }
       
    }
}
