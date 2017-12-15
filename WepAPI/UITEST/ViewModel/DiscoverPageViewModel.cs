using Entities.RedditEntities;
using Gorilla.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Model;
using UITEST.View;

namespace UITEST.ViewModel
{
    public class DiscoverPageViewModel : BaseViewModel
    {
        public ObservableCollection<Subreddit> SubReddits { get; private set; }

        public ICommand GoToSubRedditPage { get; set; }
        private readonly IRestSubredditConnectionRepository _repository;
        private readonly IRedditAPIConsumer _consumer;
        public  readonly IRestUserPreferenceRepository _userPreferenceRepository;
        public delegate void DiscoverReady();
        public event DiscoverReady DiscoverReadyEvent;

        public delegate void NoElements();
        public event NoElements NoElementsEvent;

        public DiscoverPageViewModel(INavigationService service, IRedditAPIConsumer consumer, IRestSubredditConnectionRepository repository, IRestUserPreferenceRepository userPreferenceRepository) :base(service)
        {
            _userPreferenceRepository = userPreferenceRepository;
            _consumer = consumer;
            _repository = repository;
          
            GoToSubRedditPage = new RelayCommand(o => _service.Navigate(typeof(MainPage), o));
            Initialize();
           
        }

        public async void Initialize()
        {


            User user = await _consumer.GetAccountDetailsAsync();

            var result = (await _userPreferenceRepository.FindAsync(user.name));

            if (result == null)
            {
                NoElementsEvent.Invoke();
                DiscoverReadyEvent.Invoke();
            }
            else
            {
                var userPreferences = result.ToArray();
                var subreddits = new HashSet<string>();
                if (userPreferences.Count() < 15)
                {

                    int reps = (15 / userPreferences.Count());

                    for (int j = 0; j < userPreferences.Count(); j++)
                    {
                        var SubredditConnections = await _repository.FindAsync(userPreferences[j].SubredditName);
                        if (SubredditConnections != null)
                        {
                            for (int i = 0; i < reps && i < SubredditConnections.Count(); i++)
                            {
                                subreddits.Add(SubredditConnections.ElementAt(i).SubredditToName);

                            }
                        }
                    }

                }
                else
                {

                    for (int i = 0; i < 15; i++)
                    {
                        subreddits.Add(((await _repository.FindAsync(userPreferences[i]
                                                            .SubredditName)).
                                                            FirstOrDefault()).SubredditToName);
                    }
                }


                var subs = new Subreddit[14];
                for(int i = 0; i<subreddits.Count(); i++)
                {
                    var sub = await _consumer.GetSubredditAsync(subreddits.ElementAt(i));
                    
                    subs[i] = sub;
                    if(sub.banner_img == null)
                    {
                        sub.banner_img = sub.header_img;
                        if(sub.banner_img == null)
                        {
                            sub.banner_img = sub.icon_img;
                        }
                    }
                }
                SubReddits = new ObservableCollection<Subreddit>(subs);
                DiscoverReadyEvent.Invoke();
                OnPropertyChanged("SubReddits");
            }
        }
    }
}
