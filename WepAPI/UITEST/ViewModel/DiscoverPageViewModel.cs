using Entities.RedditEntities;
using Gorilla.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Model;
using UITEST.View;
using UITEST.RedditInterfaces;
using Entities;
using System.Threading.Tasks;
using System.Timers;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace UITEST.ViewModel
{
    public class DiscoverPageViewModel : BaseViewModel
    {
        public ObservableCollection<Entities.RedditEntities.Subreddit> SubReddits { get; private set; }
        public ICommand GoToSubRedditPage { get; set; }
        private readonly IRestSubredditConnectionRepository _repository;
        private readonly IRedditAPIConsumer _consumer;
        public readonly IRestUserPreferenceRepository _userPreferenceRepository;
        public delegate void DiscoverReady();
        public event DiscoverReady DiscoverReadyEvent;
        public delegate void NoElements();
        public event NoElements NoElementsEvent;

        public DiscoverPageViewModel(INavigationService service, IRedditAPIConsumer consumer,
            IRestSubredditConnectionRepository repository,
            IRestUserPreferenceRepository userPreferenceRepository) : base(service)
        {
            _userPreferenceRepository = userPreferenceRepository;
            _consumer = consumer;
            _repository = repository;

            GoToSubRedditPage = new RelayCommand(o => _service.Navigate(typeof(MainPage), o));
        }

        public async void Initialize()
        {
            Entities.RedditEntities.User user = UserFactory.GetInfo();

            var result = (await _userPreferenceRepository.FindAsync(user.name));

            if (result == null)
            {
                NoElementsEvent.Invoke();
                DiscoverReadyEvent.Invoke();
            }
            else
            {
                var connections = await _repository.GetAllPrefs(result.Select(a => a.SubredditName).ToArray());

                var taskList = new List<Task>();
                var subs = new Entities.RedditEntities.Subreddit[connections.Count()];
                int j = 0;
                foreach (string subreddit in connections.Select(A => A.SubredditToName))
                {
                    taskList.Add(finalize(j, subreddit, subs, connections.ElementAt(j).SubredditFromName));

                    j++;
                }
                await Task.WhenAll(taskList);

                SubReddits = new ObservableCollection<Entities.RedditEntities.Subreddit>(subs);
                DiscoverReadyEvent.Invoke();
                OnPropertyChanged("SubReddits");
            }
        }
        public async Task add(int k, int reps, ConcurrentBag<string> subreddits, string subredditFromName)
        {

            var SubredditConnections = await _repository.FindAsync(subredditFromName);
            Debug.WriteLine(subredditFromName);
            if (SubredditConnections != null)
            {
                subreddits.Add(SubredditConnections.ElementAt(reps).SubredditToName);
                k++;
            }
        }

        public async Task addOver(int i, ConcurrentBag<string> subreddits, string subredditFromName)
        {
            Debug.WriteLine(subredditFromName);
            var sub = await (_repository.FindAsync(subredditFromName));
            if (sub != null)
            {
                subreddits.Add(sub.FirstOrDefault().SubredditToName);
            }
        }

        public async Task finalize(int i, string subreddit, Entities.RedditEntities.Subreddit[] subs,
            string subredditFromName)
        {
            var sub = await _consumer.GetSubredditAsync(subreddit);
            sub.interest = subredditFromName;
            subs[i] = sub;
            if (sub.banner_img.Equals(""))
            {
                sub.banner_img = sub.header_img;

            }
        }
    }
}

