﻿using Entities.RedditEntities;
using Gorilla.Model;
using Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
namespace UITEST.ViewModel
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public ObservableCollection<Post> Posts { get; private set; }

        //ProfileInformation
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

        public ICommand GoToPostPageCommand { get; set; }

        private readonly IRestUserRepository _repository;

        private readonly IRedditAPIConsumer _consumer;

        private ImageSource _image;
        public ImageSource Image { get { return _image; } set { if (_image != value) { _image = value; OnPropertyChanged(); } } }

        private byte[] _imageBytes;
        public byte[] ImageBytes { get { return _imageBytes; } set { if (_imageBytes != value) { _imageBytes = value; OnPropertyChanged(); LoadImageAsync(); } } }

        public ProfilePageViewModel(INavigationService service, IRestUserRepository repository, IRedditAPIConsumer consumer) : base(service)
        {
            _repository = repository;
            _consumer = consumer;
        }

        public async Task Initialize()
        {
           // await _repository.CreateAsync(new Entities.User {Username = "Test5", PathToProfilePicture = "profilePicture.jpg" });

            ImageBytes = await  _repository.FindImageAsync("Test5");
            
            /*
            Posts = new ObservableCollection<Post>
            {
                new Post {Title = "hej", Author = "Raaaasmusss", NumOfVotes = -100, Text = "FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. hej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gid"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
            };
            */
            
            await GetCurrentProfile();
        }

        private async Task GetCurrentProfile()
        {
            var redditUser = await _consumer.GetAccountDetailsAsync();
            var subscriptions = await _consumer.GetSubscribedSubredditsAsync();
            var userPosts = await _consumer.GetUserPosts(redditUser.name);
            string numberOfPosts;
            if (userPosts.Count > 25) numberOfPosts = "25+";
            else numberOfPosts = userPosts.Count.ToString();
            var UserComments = await _consumer.GetUserComments(redditUser.name);
            string numberOfComments;
            if (UserComments.Count > 25) numberOfComments = "25+";
            else numberOfComments = UserComments.Count.ToString();
            var unix = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            var time = unix.AddSeconds(redditUser.created);
            
            Username = redditUser.name;
            AmountOfSubRedditsSubscribedTo = subscriptions.Count();
            JoinDate = time;
            CommentKarma = redditUser.comment_karma;
            LinkKarma = redditUser.link_karma;
            PostsCreated = numberOfPosts;
            CommentsCreated = numberOfComments;
        }

        private async void GetCommentHistory()
        {
        }

        private async void GetPostHistory()
        {

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
