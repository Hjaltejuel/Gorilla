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
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace UITEST.ViewModel
{
    public class MainPageViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ICommand GoToCreatePostPageCommand { get; set; }
        bool firstTime = true;
        IRedditAPIConsumer _consumer;
        protected ISubredditRepository _repository;
        public string subredditString;
        public Subreddit subreddit;
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
        
        public async Task GeneratePosts(string s = "sircmpwn")
        {
            //try
            //{
                subreddit = await _consumer.GetSubredditAsync(s);
                Posts = subreddit.posts;
            //}
            //catch (JsonReaderException e)
            //{
            //    Debug.WriteLine(e.StackTrace);
            //    Debug.WriteLine("WHAT?");

            //}
            PostsReadyEvent.Invoke();
        }

        public async Task SubscribeToSubreddit()
        {
            await _consumer.SubscribeToSubreddit(subreddit, true);
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
