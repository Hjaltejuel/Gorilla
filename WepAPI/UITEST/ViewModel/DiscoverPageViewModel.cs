using Entities.RedditEntities;
using Gorilla.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Model;

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


            User user = await  _consumer.GetAccountDetailsAsync();
            
            var userPreferences = (await _userPreferenceRepository.FindAsync(user.name)).ToArray();

            var subreddits = new HashSet<string>();
            if (userPreferences.Count() < 15)
            {
               
                int reps = (15/userPreferences.Count());

                for (int j = 0; j < userPreferences.Count(); j++)
                {
                    var SubredditConnections = await _repository.FindAsync(userPreferences[j].SubredditName);
                    for (int i = 0; i < reps && i< SubredditConnections.Count(); i++)
                    {
                        subreddits.Add(SubredditConnections.ElementAt(i).SubredditToName);
                   
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


            SubReddits = new ObservableCollection<Subreddit>();
            foreach(String s in subreddits)
            {
                SubReddits.Add(new Subreddit() { name = s , banner_img = "/MockUpPictures/Destiny2Banner.png"});
            }
            DiscoverReadyEvent.Invoke();
            OnPropertyChanged("SubReddits");
        }
    }
}
