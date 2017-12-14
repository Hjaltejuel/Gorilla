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
using System.Collections.Generic;

namespace UITEST.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        public ICommand GoToCreatePostPageCommand { get; set; }
        bool firstTime = true;
        IRedditAPIConsumer _consumer;
        protected ISubredditRepository _repository;
        public Subreddit subreddit;
        public ObservableCollection<Post> posts;
        public string subscribeString = "Subscribe";
        bool userIsSubscribed;
        bool UserIsSubscribed {
            get => userIsSubscribed;
            set
            {
                userIsSubscribed = value;
                subscribeString = value ? "Subscribe" : "Unsubscribe";
                OnPropertyChanged("subscribeString");
            }
        }
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
            _repository = repository;
            _helper = helper;
            GoToCreatePostPageCommand = new RelayCommand(o => _service.Navigate(typeof(CreatePostPage), subreddit));
            GeneratePosts();
        }
        
        public async Task GeneratePosts(string s = "sircmpwn", string sort = "hot")
        {
           
                subreddit = await _consumer.GetSubredditAsync(s, sort);
                Posts = subreddit.posts;
          
            List<Subreddit> subs = await _consumer.GetSubscribedSubredditsAsync();
            UserIsSubscribed = !subs.Contains(subreddit);
            PostsReadyEvent.Invoke();
        }

        public async Task SubscribeToSubreddit()
        {
            UserIsSubscribed = !UserIsSubscribed;
            await _consumer.SubscribeToSubreddit(subreddit, !UserIsSubscribed);
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
