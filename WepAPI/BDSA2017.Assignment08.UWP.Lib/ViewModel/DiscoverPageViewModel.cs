using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
        }

        public async Task Initialize()
        {
            var user = _userHandler.GetUser();

            var result = (await UserPreferenceRepository.FindAsync(user.name));
            if (result == null) NoElementsEvent?.Invoke();
            var top15Result = result.OrderBy(e => e.PriorityMultiplier).Take(15);
            if (top15Result == null)
            {
                NoElementsEvent?.Invoke();
            }
            else
            {
                var connections = await _repository.GetAllPrefs(top15Result.Select(a => a.SubredditName).ToArray());
                if (connections == null) NoElementsEvent?.Invoke();
                {
                    var taskList = new List<Task>();
                    var subs = new Dictionary<string, Subreddit>();
                    foreach (var subreddit in connections)
                    {
                        taskList.Add(Finalize(subreddit.SubredditToName, subs, subreddit.SubredditFromName));
                    }
                    await Task.WhenAll(taskList);

                    SubReddits = new ObservableCollection<Subreddit>(subs.Values);
                    OnPropertyChanged("SubReddits");
                    DiscoverReadyEvent?.Invoke();
                }
            }
        }
        public async Task Finalize(string subreddit, Dictionary<string,Subreddit> subs, string subredditFromName)
        {
            var sub = (await _consumer.GetSubredditAsync(subreddit)).Item2;
            sub.interest = subredditFromName;
            subs.TryAdd(sub.display_name,sub);
            if (string.IsNullOrEmpty(sub.banner_img))
            {
                sub.banner_img = "";
            }
        }


        public async Task<Subreddit> GetSubredditPosts(Subreddit subreddit)
        {
            var subredditPostsResult = await _consumer.GetSubredditPostsAsync(subreddit);
            if (subredditPostsResult.Item1 == HttpStatusCode.OK)
            {
                subreddit = (subredditPostsResult).Item2;
                return subreddit;
            }
            return null;
        }
    }
}

