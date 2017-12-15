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
using Gorilla.Model.GorillaRestInterfaces;

namespace UITEST.ViewModel
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public ObservableCollection<Entities.RedditEntities.Post> Posts { get; private set; }
        

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
        private readonly IRestPostRepository _restPostRepository;

        private readonly IRedditAPIConsumer _consumer;

        private ImageSource _image;
        public ImageSource Image { get { return _image; } set { if (_image != value) { _image = value; OnPropertyChanged(); } } }

        private byte[] _imageBytes;
        public byte[] ImageBytes { get { return _imageBytes; } set { if (_imageBytes != value) { _imageBytes = value; OnPropertyChanged(); LoadImageAsync(); } } }

        public ProfilePageViewModel(INavigationService service, IRestUserRepository repository, IRedditAPIConsumer consumer, IRestPostRepository restPostRepository) : base(service)
        {
            _restPostRepository = restPostRepository;

            _repository = repository;

            _consumer = consumer;

            Initialize();
        }

        private async void Initialize()
        {

          

           

           

           
           

            
            
            await GetCurrentProfile();

            await _repository.CreateAsync(new Entities.User { Username = currentProfile.Username, PathToProfilePicture = "profilePicture.jpg" });

            var ImageBytes = await _repository.FindImageAsync(currentProfile.Username);

            var postIds = await _restPostRepository.ReadAsync(currentProfile.Username);



            Posts = new ObservableCollection<Entities.RedditEntities.Post>();
           
            Parallel.ForEach(postIds, async post => { Posts.Add(await _consumer.GetPostAndCommentsByIdAsync(post.Id)); });

            OnPropertyChanged();
       
        }

        private async Task GetCurrentProfile()
        {
            
            var redditUser = await _consumer.GetAccountDetailsAsync();
            var subscriptions = await _consumer.GetSubscribedSubredditsAsync();

           

            var unix = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            var time = unix.AddSeconds(redditUser.created);
            CurrentProfile = new Profile()
            {
                Name = "Morten",
                Username = redditUser.name,
                Email = "thereddestroyer@gmail.com",
                AmountOfSubRedditsSubscribedTo = subscriptions.Count(),
                JoinDate = time,
                CommentKarma = redditUser.comment_karma,
                LinkKarma = redditUser.link_karma,
                PostCreated = 54,
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
