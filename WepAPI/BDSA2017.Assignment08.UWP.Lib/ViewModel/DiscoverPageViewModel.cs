using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Entities.RedditEntities;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;

namespace UI.Lib.ViewModel
{
    public class DiscoverPageViewModel : BaseViewModel
    {
        public ObservableCollection<Subreddit> SubReddits { get; private set; }
        public ICommand GoToSubRedditPage { get; set; }
        private readonly IRestSubredditConnectionRepository _repository;
        private readonly IRedditApiConsumer _consumer;
        public readonly IRestUserPreferenceRepository UserPreferenceRepository;
        public delegate void DiscoverReady();
        public event DiscoverReady DiscoverReadyEvent;
        public delegate void NoElements();
        public event NoElements NoElementsEvent;
        private readonly IUserHandler _userHandler;

        public DiscoverPageViewModel(
            INavigationService service, IRedditApiConsumer consumer,
            IRestSubredditConnectionRepository repository,
            IRestUserPreferenceRepository userPreferenceRepository, 
            IUserHandler userHandler)
            : base(service)
        {
            UserPreferenceRepository = userPreferenceRepository;
            _consumer = consumer;
            _repository = repository;
            _userHandler = userHandler;

            GoToSubRedditPage = new RelayCommand(o => Service.Navigate(SubredditPage, o));
        }

        public async void Initialize()
        {
            var user = _userHandler.GetUser();

            var result = (await UserPreferenceRepository.FindAsync(user.name));

            if (result == null) NoElementsEvent?.Invoke();
            else
            {
                var connections = await _repository.GetAllPrefs(result.Select(a => a.SubredditName).ToArray());

                var taskList = new List<Task>();
                var subs = new List<Subreddit>();
                foreach (var subreddit in connections)
                {
                    taskList.Add(Finalize(subreddit.SubredditToName, subs, subreddit.SubredditFromName));
                }
                await Task.WhenAll(taskList);

                SubReddits = new ObservableCollection<Subreddit>(subs);
                OnPropertyChanged("SubReddits");
            }
            DiscoverReadyEvent?.Invoke();
        }
        public async Task Finalize(string subreddit, List<Subreddit> subs, string subredditFromName)
        {
            var sub = (await _consumer.GetSubredditAsync(subreddit)).Item2;
            sub.interest = subredditFromName;
            subs.Add(sub);
            if (string.IsNullOrEmpty(sub.banner_img))
            {
                sub.banner_img = "";
            }
        }
    }
}

