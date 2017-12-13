using Entities.RedditEntities;
using Gorilla.AuthenticationGorillaAPI;
using Gorilla.Model;
using Gorilla.View;
using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UITEST.View;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Controls;

namespace UITEST.ViewModel
{
    public class MainPageViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ICommand GoToCreatePostPageCommand { get; set; }
        bool firstTime = true;
        IRedditAPIConsumer _consumer;
        protected ISubredditRepository _repository;
        public Subreddit subreddit;
        public ObservableCollection<Post> Posts { get; set; }

        public delegate void PostsReady();
        public event PostsReady PostsReadyEvent;

        public MainPageViewModel(ISubredditRepository repository, IAuthenticationHelper helper, INavigationService service, IRedditAPIConsumer consumer) : base(service)
        {
            _consumer = consumer;
            _consumer.RefreshTokenAsync();
            //bool hasToken = _consumer.RefreshTokenAsync().Result;
            //if (!hasToken)
            //{
            //    //TODO
            //    //FUUUUUCK 
            //}
            _repository = repository;
            _helper = helper;

            GoToCreatePostPageCommand = new RelayCommand(o => _service.Navigate(typeof(CreatePostPage), subreddit));
        }

        public async void GeneratePosts()
        {
            subreddit = await _consumer.GetSubredditAsync("AskReddit");
            PostsReadyEvent.Invoke();
            Posts = subreddit.posts;
            OnPropertyChanged("Posts");
        }

        public async Task Initialize()
        {
            if (await Authorize() != null)
            {
                GeneratePosts();
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
