using Entities.RedditEntities;
using Gorilla.AuthenticationGorillaAPI;
using Gorilla.Model;
using Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UITEST.RedditInterfaces;
using UITEST.View;

namespace UITEST.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        public ICommand GoToCreatePostPageCommand { get; set; }
        bool firstTime = true;
        IRedditAPIConsumer _consumer;
        IRestUserRepository _repository;
        private string _queryText;
        public string queryText{ get { return _queryText; } set { if (_queryText != value) { _queryText = value; OnPropertyChanged(); } }}

        public ObservableCollection<Post> posts;
        public delegate void LoadingEvent();
        public event LoadingEvent LoadSwitch;
        public ObservableCollection<Post> Posts
        {
            get => posts;
            set
            {
                posts = value;
                OnPropertyChanged("Posts");
            }
        }


        public MainPageViewModel(IAuthenticationHelper helper, INavigationService service, IRedditAPIConsumer consumer, IRestUserRepository repository) : base(service)
        {
            _consumer = consumer;
            _repository = repository;
            _helper = helper;
            Initialize();
        }

        public async Task GeneratePosts()
        {
            LoadSwitch.Invoke();
            await UserFactory.initialize(_consumer);
            await _repository.CreateAsync(new Entities.User { Username = UserFactory.GetInfo().name, PathToProfilePicture = "profilePicture.jpg" });
            Posts = await _consumer.GetHomePageContent();
            foreach (Post p in Posts)
            {
                if (p.is_self)
                {
                    p.thumbnail = "/Assets/Textpost.png";
                }
                else
                {
                    if (p.thumbnail == "default")
                    {
                        p.thumbnail = "/Assets/Externallink.png";
                    }
                }
            }
            LoadSwitch.Invoke();
        }
        public async Task Initialize()
        {
            if (await Authorize() != null)
            {
                await GeneratePosts();
            }
            else
            {
                if(firstTime == true)
                {
                    firstTime = false;
                }
                else
                {
                    await Initialize();
                }
            }
        }

        public void SearchQuerySubmitted()
        {
            _service.Navigate(typeof(SubredditPage), queryText);
        }


    }
}
