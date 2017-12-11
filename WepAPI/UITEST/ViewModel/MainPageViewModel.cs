using Entities.RedditEntities;
using Gorilla.AuthenticationGorillaAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UITEST.Model;

namespace UITEST.ViewModel
{
    public class MainPageViewModel : BaseViewModel, INotifyPropertyChanged
    {
        IRedditAPIConsumer APIConsumer;
        public ObservableCollection<Post> Posts { get; set; }
        private IAuthenticationHelper _helper;

        public delegate void PostsReady();
        public event PostsReady PostsReadyEvent;

        public MainPageViewModel()
        {
            Initialize();
        }

        public async void GeneratePosts()
        {
            APIConsumer = new RedditConsumerController();
            Subreddit subreddit = await APIConsumer.GetSubredditAsync("AskReddit");
            PostsReadyEvent.Invoke();
            Posts = subreddit.posts;
            OnPropertyChanged("Posts");
        }

        public void Initialize()
        {
            GeneratePosts();
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
    }
}
