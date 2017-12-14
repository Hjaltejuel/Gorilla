using Entities.RedditEntities;
using Gorilla.AuthenticationGorillaAPI;
using Gorilla.Model;
using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using UITEST.View;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Controls;

namespace UITEST.ViewModel
{
    public class MainPageViewModel : BaseViewModel, INotifyPropertyChanged
    {
        bool firstTime = true;
        IRedditAPIConsumer _consumer;
        protected IRestSubredditRepository _repository;
        public ObservableCollection<Post> Posts { get; set; }

        public delegate void PostsReady();
        public event PostsReady PostsReadyEvent;

        public MainPageViewModel(IRestSubredditRepository repository, IAuthenticationHelper helper, INavigationService service, IRedditAPIConsumer consumer) : base(service)
        {
            _consumer = consumer;
            _repository = repository;
            _helper = helper;

            Initialize();
         
            
        }

        public async void GeneratePosts()
        {

          
            Subreddit subreddit = await _consumer.GetSubredditAsync("AskReddit");
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
