using Entities.RedditEntities;
using Gorilla.AuthenticationGorillaAPI;
using Gorilla.Model;
using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UITEST.View;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Controls;

namespace UITEST.ViewModel
{
    public class MainPageViewModel : BaseViewModel, INotifyPropertyChanged
    {
        IRedditAPIConsumer _consumer;
        public ObservableCollection<Post> Posts { get; set; }
        private IAuthenticationHelper _helper;

        public delegate void PostsReady();
        public event PostsReady PostsReadyEvent;

        public MainPageViewModel(ISubredditRepository repository, IAuthenticationHelper helper, INavigationService service, IRedditAPIConsumer consumer) : base(service)
        {
            _consumer = consumer;
            _repository = repository;
            _helper = helper;

    

            Authorize();

            Initialize();

           
                
            
        }

        public async void GeneratePosts()
        {
          
           
            
          
            Subreddit subreddit = await _consumer.GetSubredditAsync("AskReddit");
            PostsReadyEvent.Invoke();
            Posts = subreddit.posts;
            OnPropertyChanged("Posts");
        }

        public  void Initialize()
        {  
 
          GeneratePosts();
         
            
        
        }
    }
}
