using Entities.RedditEntities;
using Gorilla.Model;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UITEST.Model;
using UITEST.View;
using Entities;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace UITEST.ViewModel
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public ObservableCollection<Post> Posts { get; private set; }
        

        private Profile currentProfile;

        public Profile CurrentProfile
        {
            get => currentProfile;
            set
            {
                currentProfile = value;
                OnPropertyChanged("CurrentProfile");
            }
        }

        public ICommand GoToPostPageCommand { get; set; }

        private readonly IRestUserRepository _repository;

        private ImageSource _image;
        public ImageSource Image { get { return _image; } set { if (_image != value) { _image = value; OnPropertyChanged(); } } }

        private byte[] _imageBytes;
        public byte[] ImageBytes { get { return _imageBytes; } set { if (_imageBytes != value) { _imageBytes = value; OnPropertyChanged(); LoadImageAsync(); } } }

        public ProfilePageViewModel(INavigationService service, IRestUserRepository repository) : base(service)
        {
            _repository = repository;

            Initialize();
        }

        private async void Initialize()
        {

          

           // await _repository.CreateAsync(new Entities.User {Username = "Test5", PathToProfilePicture = "profilePicture.jpg" });

            CurrentProfile = new Profile() { Name = "Morten", Username = "RedRabbitRasmusRaarup", Email = "thereddestroyer@gmail.com", AmountOfSubRedditsSubscribedTo = 12, JoinDate = new DateTime(2015, 02, 15), KarmaGiven = 201, KarmaRecieved = 635, PostCreated = 54 };

            ImageBytes = await  _repository.FindImageAsync("Test5");
            
            Posts = new ObservableCollection<Post>
            {
                new Post {Title = "hej", Author = "Raaaasmusss", NumOfVotes = -100, Text = "FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. hej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gid"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
            };
            */
            GetCurrentProfile();
        }

        private async void GetCurrentProfile()
        {
            var redditAPIConsumer = App.ServiceProvider.GetService<IRedditAPIConsumer>();
            var redditUser = await redditAPIConsumer.GetAccountDetailsAsync();
            var subscriptions = await redditAPIConsumer.GetSubscribedSubredditsAsync();
            var unix = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            var time = unix.AddSeconds(redditUser.created);
            CurrentProfile = new Profile()
            {
                Name = "Morten",
                Username = redditUser.name,
                Email = "thereddestroyer@gmail.com",
                AmountOfSubRedditsSubscribedTo = subscriptions.Count,
                JoinDate = time,
                CommentKarma = redditUser.comment_karma,
                LinkKarma = redditUser.link_karma,
                PostCreated = 54,
                PathToProfilePicture = redditUser.icon_img
            }; 
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
