using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gorilla.Model;
using Entities.RedditEntities;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Gorilla.AuthenticationGorillaAPI;
using Gorilla.View;
using UITEST.RedditInterfaces;
using Model;

namespace UITEST.ViewModel
{
    class SubredditPageViewModel : BaseViewModel
    {
        public ICommand GoToCreatePostPageCommand { get; set; }
        bool firstTime = true;
        IRedditAPIConsumer _consumer;

        public Subreddit _Subreddit;
        public ObservableCollection<Post> posts;
        private List<string> _SortTypes;

        public List<string> SortTypes
        {
            get { return _SortTypes; }
            set { _SortTypes = value; OnPropertyChanged(); }
        }


        private string  _subscribeString;
        public string subscribeString { get { return _subscribeString; } set { _subscribeString = value;  OnPropertyChanged(); }}

        private string _subredditName;
        public string SubredditName
        {
            get { return _subredditName; }
            set
            {
                if (value != _subredditName)
                {
                    _subredditName = value;
                    OnPropertyChanged();
                }
            }
        }
        IRestUserPreferenceRepository _repository;
        public delegate void PostsReady();
        public event PostsReady PostsReadyEvent;

        public SubredditPageViewModel(IAuthenticationHelper helper, INavigationService service, IRedditAPIConsumer consumer, IRestUserPreferenceRepository repository ) : base(service)
        {
            _repository = repository;
            _consumer = consumer;
            _helper = helper;
            GoToCreatePostPageCommand = new RelayCommand(o => _service.Navigate(typeof(CreatePostPage), _Subreddit));
            SortTypes = new List<string>() { "hot", "new", "rising", "top", "controversial" };

        }
        bool userIsSubscribed;
        bool UserIsSubscribed
        {
            get => userIsSubscribed;
            set
            {
                userIsSubscribed = value;
                subscribeString = value ? "Unsubscribe" : "Subscribe";
                OnPropertyChanged("subscribeString");
            }
        }
        public ObservableCollection<Post> Posts
        {
            get => posts; set { posts = value; OnPropertyChanged("Posts"); }
        }

        public async Task GeneratePosts(string subredditName, string sort = "hot")
        {
            _Subreddit = await _consumer.GetSubredditAsync(subredditName, sort);
            if(_Subreddit==null || _Subreddit.name == null)
            {
                return;
            }
            SubredditName = _Subreddit.display_name_prefixed;

            Posts = _Subreddit.posts;

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
            List<Subreddit> subs = await _consumer.GetSubscribedSubredditsAsync();
            UserIsSubscribed = (from b in subs
                                where b.display_name.Equals(_Subreddit.display_name)
                                select b).Any();
            PostsReadyEvent.Invoke();
        }

        public async Task SubscribeToSubreddit()
        {
            UserIsSubscribed = !UserIsSubscribed;
            await _consumer.SubscribeToSubreddit(_Subreddit, UserIsSubscribed);
            if (UserIsSubscribed)
            {
                await _repository.UpdateAsync(new Entities.UserPreference { Username = UserFactory.GetInfo().name, SubredditName = _Subreddit.display_name, PriorityMultiplier = 10 });

            } else
            {
                await _repository.UpdateAsync(new Entities.UserPreference { Username = UserFactory.GetInfo().name, SubredditName = _Subreddit.display_name, PriorityMultiplier = -10 });
            }
        }

        public async Task Initialize()
        {
            if (await Authorize() != null)
            {
                //GeneratePosts();
            }
            else
            {
                if (firstTime == true)
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