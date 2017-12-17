using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Entities.GorillaEntities;
using UITEST.Authentication.GorillaAuthentication;
using UITEST.Model;
using UITEST.Model.GorillaRestInterfaces;
using UITEST.Model.RedditRestInterfaces;
using CreatePostPage = UITEST.View.CreatePostPage;
using Subreddit = Entities.RedditEntities.Subreddit;

namespace UITEST.ViewModel
{
    class SubredditPageViewModel : SearchableViewModel
    {
        public ICommand GoToCreatePostPageCommand { get; set; }
        public ICommand SubscribeToSubredditCommand { get; set; }

        private readonly IRestUserPreferenceRepository _repository;
        public Subreddit _Subreddit;
        private List<string> _SortTypes;
        public List<string> SortTypes
        {
            get => _SortTypes;
            set { _SortTypes = value; OnPropertyChanged(); }
        }

        private string  _subscribeString;
        public string SubscribeString { get => _subscribeString;
            set { _subscribeString = value;  OnPropertyChanged(); }}

        private string _subredditName;
        public string SubredditName { get => _subredditName;
            set  { if (value != _subredditName){ _subredditName = value;OnPropertyChanged();}}}

        bool _userIsSubscribed;
        bool UserIsSubscribed
        {
            get => _userIsSubscribed;
            set
            {
                _userIsSubscribed = value;
                SubscribeString = value ? "Unsubscribe" : "Subscribe";
                OnPropertyChanged("subscribeString");
            }
        }
        public SubredditPageViewModel(IAuthenticationHelper helper, INavigationService service, IRedditApiConsumer consumer, IRestUserPreferenceRepository repository ) : base(helper, service, consumer)
        {
            _repository = repository;
            GoToCreatePostPageCommand = new RelayCommand(o => Service.Navigate(typeof(CreatePostPage), _Subreddit));
            SubscribeToSubredditCommand = new RelayCommand(async o => { await SubscribeToSubreddit(); });
            SortTypes = new List<string>() { "hot", "new", "rising", "top", "controversial" };
        }
        public async Task GeneratePosts(string subredditName, string sort = "hot")
        {
            InvokeLoadSwitchEvent();
            _Subreddit = (await Consumer.GetSubredditAsync(subredditName)).Item2;
            _Subreddit = (await Consumer.GetSubredditPostsAsync(_Subreddit, sort)).Item2;
            if (_Subreddit?.name == null)
            {
                InvokeLoadSwitchEvent();
                return;
            }
            SubredditName = _Subreddit.display_name_prefixed;
            Posts = _Subreddit.posts;
            await IsUserSubscribed();
            InvokeLoadSwitchEvent();
        }

        private async Task IsUserSubscribed()
        {
            List<Subreddit> subs = (await Consumer.GetSubscribedSubredditsAsync()).Item2;
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
                await _repository.UpdateAsync(new UserPreference { Username = UserFactory.GetInfo().name, SubredditName = _Subreddit.display_name, PriorityMultiplier = 10 });

            } else
            {
                await _repository.UpdateAsync(new UserPreference { Username = UserFactory.GetInfo().name, SubredditName = _Subreddit.display_name, PriorityMultiplier = -10 });
            }
        }
    }
}