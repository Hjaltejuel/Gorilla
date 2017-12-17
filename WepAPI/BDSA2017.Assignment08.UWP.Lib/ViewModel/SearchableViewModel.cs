using System.Collections.ObjectModel;
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
        private string _queryText;
        public string QueryText { get => _queryText;
            set { if (_queryText != value) { _queryText = value; OnPropertyChanged(); } } }

        private ObservableCollection<Post> posts;
        public ObservableCollection<Post> Posts
        {
            get => posts; set { posts = value; OnPropertyChanged(); }
        }
        protected SearchableViewModel(IAuthenticationHelper helper, INavigationService service, IRedditApiConsumer consumer) : base(service)
        {
            Consumer = consumer;
            Helper = helper;
        }

        public void SearchQuerySubmitted()
        {
            Service.Navigate(SubredditPage, QueryText);
        }

        public void InvokeLoadSwitchEvent()
        {
            LoadSwitch?.Invoke();
        }
    }
}

