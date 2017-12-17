using Entities.RedditEntities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using UITEST.View;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;
using UITEST.Model;
using UITEST.Model.GorillaRestInterfaces;
using UITEST.Model.RedditRestInterfaces;

namespace UITEST.ViewModel
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

        public DiscoverPageViewModel(INavigationService service, IRedditApiConsumer consumer,
            IRestSubredditConnectionRepository repository,
            IRestUserPreferenceRepository userPreferenceRepository) : base(service)
        {
            UserPreferenceRepository = userPreferenceRepository;
            _consumer = consumer;
            _repository = repository;

            GoToSubRedditPage = new RelayCommand(o => Service.Navigate(typeof(SubredditPage), o));
        }

        public async void Initialize()
        {
            var user = UserFactory.GetInfo();

            var result = (await UserPreferenceRepository.FindAsync(user.name));

            if (result == null)
            {
                NoElementsEvent?.Invoke();
                DiscoverReadyEvent?.Invoke();
            }
            else
            {
                var connections = await _repository.GetAllPrefs(result.Select(a => a.SubredditName).ToArray());

                var taskList = new List<Task>();
                var subs = new Subreddit[connections.Count];
                var j = 0;
                foreach (var subreddit in connections.Select(a => a.SubredditToName))
                {
                    taskList.Add(Finalize(j, subreddit, subs, connections.ElementAt(j).SubredditFromName));

                    j++;
                }
                await Task.WhenAll(taskList);

                SubReddits = new ObservableCollection<Subreddit>(subs);
                DiscoverReadyEvent?.Invoke();
                OnPropertyChanged("SubReddits");
            }
        }
        public async Task Add(int k, int reps, ConcurrentBag<string> subreddits, string subredditFromName)
        {
            var subredditConnections = await _repository.FindAsync(subredditFromName);
            Debug.WriteLine(subredditFromName);
            if (subredditConnections != null)
            {
                subreddits.Add(subredditConnections.ElementAt(reps).SubredditToName);
            }
        }

        public async Task AddOver(int i, ConcurrentBag<string> subreddits, string subredditFromName)
        {
            Debug.WriteLine(subredditFromName);
            var sub = await (_repository.FindAsync(subredditFromName));
            if (sub != null)
            {
                subreddits.Add(sub.FirstOrDefault()?.SubredditToName);
            }
        }

        public async Task Finalize(int i, string subreddit, Subreddit[] subs,
            string subredditFromName)
        {
            var sub = (await _consumer.GetSubredditAsync(subreddit)).Item2;
            sub.interest = subredditFromName;
            subs[i] = sub;
            if (sub.banner_img.Equals(""))
            {
                sub.banner_img = sub.header_img;

            }
        }
    }
}

