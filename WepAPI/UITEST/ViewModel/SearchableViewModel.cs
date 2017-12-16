using System.Collections.ObjectModel;
using Entities.RedditEntities;
using UITEST.Authentication.GorillaAuthentication;
using UITEST.Model;
using UITEST.Model.GorillaRestInterfaces;
using UITEST.Model.RedditRestInterfaces;
using UITEST.View;

namespace UITEST.ViewModel
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
            Service.Navigate(typeof(SubredditPage), QueryText);
        }

        public void InvokeLoadSwitchEvent()
        {
            LoadSwitch?.Invoke();
        }
    }
}

