using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using Entities.RedditEntities;
using UI.Lib.Authentication.GorillaAuthentication;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;


namespace UI.Lib.ViewModel
{ 
    public abstract class SearchableViewModel : BaseViewModel
    {
        public delegate void LoadingEvent();
        public event LoadingEvent LoadSwitch;
        public bool FirstTime = true;
        public IRedditApiConsumer Consumer;
        public IRestUserRepository Repository;
        private IRestSubredditRepository _restSubredditRepository;

        public Subreddit _Subreddit;
        private ObservableCollection<Post> posts;
        public ObservableCollection<Post> Posts
        {
            get => posts;
            set
            {
                posts = value;
                OnPropertyChanged();
            }
        }
        protected SearchableViewModel(INavigationService service, IRedditApiConsumer consumer, IRestSubredditRepository restSubredditRepository) : base(service)
        {
            Consumer = consumer;
            _restSubredditRepository = restSubredditRepository;
        }

        private async Task<Subreddit> GetSubredditAndPostsFromName(string QueryText)
        {
            var searchForSubredditResult = await Consumer.GetSubredditAsync(QueryText);
            if (searchForSubredditResult.Item1 == HttpStatusCode.OK)
            {
                var subreddit = (searchForSubredditResult).Item2;
                var subredditPostsResult = await Consumer.GetSubredditPostsAsync(subreddit);
                if (subredditPostsResult.Item1 == HttpStatusCode.OK)
                {
                    subreddit = (subredditPostsResult).Item2;
                    return subreddit;
                }
            }
            return null;
        }

        public async Task SearchQuerySubmitted(string QueryText)
        {
            var searchedSubreddit = await GetSubredditAndPostsFromName(QueryText);
            Service.Navigate(SubredditPage, (searchedSubreddit, QueryText));
        }

        public void InvokeLoadSwitchEvent()
        {
            LoadSwitch?.Invoke();
        }
        
        public async Task<IReadOnlyCollection<string>> GetFiltered(string like)
        {
            return await _restSubredditRepository.GetLikeAsync(like);
        }
    }
}

