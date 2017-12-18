using Entities.GorillaEntities;
using Entities.RedditEntities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using UI.Lib.Authentication.GorillaAuthentication;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;
using UI.Lib.ViewModel;

namespace UI.Lib.ViewModel
{
    public class SubredditPageViewModel : SearchableViewModel
    {
        public ICommand GoToCreatePostPageCommand { get; set; }
        public ICommand SubscribeToSubredditCommand { get; set; }
        private readonly IUserHandler _userHandler;
        private readonly IRestUserPreferenceRepository _repository;
        private IRestSubredditRepository _restSubredditRepository;
        private List<string> _SortTypes;
        public List<string> SortTypes{ get => _SortTypes;
            set { _SortTypes = value; OnPropertyChanged(); }
        }

        private string _subscribeString = "Subscribe";
        public string SubscribeString { get => _subscribeString;
            set { if (value != _subscribeString) { _subscribeString = value; OnPropertyChanged(); } }
        }

        private string _subredditName;
        public string SubredditName { get => _subredditName;
            set  { if (value != _subredditName) { _subredditName = value; OnPropertyChanged();}}}

        private bool _userIsSubscribed;
        public bool UserIsSubscribed { get => _userIsSubscribed;
            set
            {
                _userIsSubscribed = value;
                SubscribeString = value ? "Unsubscribe" : "Subscribe";
                OnPropertyChanged("subscribeString");
            }
        }

        public SubredditPageViewModel(IAuthenticationHelper helper, INavigationService service, IRedditApiConsumer consumer, IRestUserPreferenceRepository repository, IUserHandler userHandler, IRestSubredditRepository restSubredditRepository) : base(service, consumer, restSubredditRepository)
        {
            _restSubredditRepository = restSubredditRepository;
            _userHandler = userHandler;
            _repository = repository;
            GoToCreatePostPageCommand = new RelayCommand(o => Service.Navigate(CreatePostPage, _Subreddit));
            SubscribeToSubredditCommand = new RelayCommand(async o => { await SubscribeToSubreddit(); });
            SortTypes = new List<string>() { "hot", "new", "rising", "top", "controversial" };
        }
        public async Task GeneratePosts(string sort = "hot")
        {
            _Subreddit = (await Consumer.GetSubredditPostsAsync(_Subreddit, sort)).Item2;
            if (!string.IsNullOrEmpty(_Subreddit?.name))
            {
                Posts = _Subreddit.posts;
                await IsUserSubscribed();
            }
            InvokeLoadSwitchEvent();
        }

        private async Task IsUserSubscribed()
        {
            var subs = (await Consumer.GetSubscribedSubredditsAsync()).Item2;
            UserIsSubscribed = (from b in subs
                                where b.display_name.Equals(_Subreddit.display_name)
                                select b).Any();
        }

        public async Task SubscribeToSubreddit()
        {
            UserIsSubscribed = !UserIsSubscribed;
            await Consumer.SubscribeToSubreddit(_Subreddit, UserIsSubscribed);
            if (UserIsSubscribed)
            {
                await _repository.UpdateAsync(new UserPreference { Username = _userHandler.GetUser().name, SubredditName = _Subreddit.display_name, PriorityMultiplier = 10 });

            } else
            {
                await _repository.UpdateAsync(new UserPreference { Username = _userHandler.GetUser().name, SubredditName = _Subreddit.display_name, PriorityMultiplier = -10 });
            }
        }

        public async void SortBy(string sortMethod)
        {
            await GeneratePosts(sortMethod);
        }
    }
}