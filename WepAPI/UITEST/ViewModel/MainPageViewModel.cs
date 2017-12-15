using Entities.RedditEntities;
using Gorilla.AuthenticationGorillaAPI;
using Gorilla.Model;
using Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UITEST.RedditInterfaces;

namespace UITEST.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        public ICommand GoToCreatePostPageCommand { get; set; }
        bool firstTime = true;
        IRedditAPIConsumer _consumer;
        IRestUserRepository _repository;
        public delegate void LoadingEvent();
        public event LoadingEvent PostsReadyEvent;
        public event LoadingEvent PostsStartedLoading;
        public ObservableCollection<Post> posts;
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
            PostsStartedLoading.Invoke();
            await UserFactory.initialize(_consumer);
            await _repository.CreateAsync(new Entities.User { Username = UserFactory.GetInfo().name, PathToProfilePicture = "profilePicture.jpg" });
            Posts = await _consumer.GetHomePageContent();
            PostsReadyEvent.Invoke();
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
    }
}
