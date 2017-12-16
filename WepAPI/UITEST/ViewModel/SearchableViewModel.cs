using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gorilla.Model;
using UITEST.ViewModel;
using System.Windows.Input;
using UITEST.RedditInterfaces;
using Model;
using System.Collections.ObjectModel;
using Entities.RedditEntities;
using Gorilla.AuthenticationGorillaAPI;
using Gorilla.View;
using UITEST.View;

namespace UITEST.ViewModel
{
    public abstract class SearchableViewModel : BaseViewModel
    {
        public delegate void LoadingEvent();
        public event LoadingEvent LoadSwitch;
        public bool firstTime = true;
        public IRedditAPIConsumer _consumer;
        public IRestUserRepository _repository;
        private string _queryText;
        public string queryText { get { return _queryText; } set { if (_queryText != value) { _queryText = value; OnPropertyChanged(); } } }

        public ObservableCollection<Post> posts;
        public ObservableCollection<Post> Posts
        {
            get => posts; set { posts = value; OnPropertyChanged("Posts"); }
        }
        public SearchableViewModel(IAuthenticationHelper helper, INavigationService service, IRedditAPIConsumer consumer) : base(service)
        {
            _consumer = consumer;
            _helper = helper;
        }

        public void SearchQuerySubmitted()
        {
            _service.Navigate(typeof(SubredditPage), queryText);
        }

        public void InvokeLoadSwitchEvent()
        {
            LoadSwitch.Invoke();
        }
    }
}

