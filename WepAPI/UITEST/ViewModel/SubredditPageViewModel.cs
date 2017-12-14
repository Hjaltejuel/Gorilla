using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gorilla.Model;
using Entities.RedditEntities;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Gorilla.AuthenticationGorillaAPI;
using Gorilla.View;

namespace UITEST.ViewModel
{
    class SubredditPageViewModel : BaseViewModel
    {
        public ICommand GoToCreatePostPageCommand { get; set; }
        bool firstTime = true;
        IRedditAPIConsumer _consumer;

        public Subreddit _Subreddit;
        public ObservableCollection<Post> posts;
        public string subscribeString = "Subscribe";
        bool userIsSubscribed;
        public delegate void PostsReady();
        public event PostsReady PostsReadyEvent;

        public SubredditPageViewModel(IAuthenticationHelper helper, INavigationService service, IRedditAPIConsumer consumer) : base(service)
        {
            _consumer = consumer;
            _helper = helper;
            GoToCreatePostPageCommand = new RelayCommand(o => _service.Navigate(typeof(CreatePostPage), _Subreddit));
        }
        bool UserIsSubscribed
        {
            get => userIsSubscribed;
            set
            {
                userIsSubscribed = value;
                subscribeString = value ? "Subscribe" : "Unsubscribe";
                OnPropertyChanged("subscribeString");
            }
        }
        public ObservableCollection<Post> Posts
        {
            get => posts;
            set
            {
                posts = value;
                OnPropertyChanged("Posts");
            }
        }


        public async Task GeneratePosts(string subredditName, string sort = "hot")
        {
            //if (_vm._Subreddit.display_name == null)
            //{
            //    PageTitleText.Text = "";
            //    NothingFoundTextBlock = new TextBlock() { Text = $"Nothing Found on r/{SubredditToSearchFor}", FontSize = 50, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            //    _Grid.Children.Add(NothingFoundTextBlock);
            //    Grid.SetRow(NothingFoundTextBlock, 3);
            //}
            //else
            //{
            //    PageTitleText.Text = _vm._Subreddit.display_name_prefixed;
            //}
            //_vm._Subreddit = e.Parameter as Subreddit;
            //if (_vm._Subreddit != null)
            //    PageTitleText.Text = _vm._Subreddit.display_name_prefixed;

            _Subreddit = await _consumer.GetSubredditAsync(subredditName, sort);
            if(_Subreddit==null || _Subreddit.name == null)
            {
                return;
            }

            Posts = _Subreddit.posts;

            foreach (Post p in Posts)
            {
                if (p.is_self)
                {
                    p.thumbnail = "/Assets/Textpost.png";
                }
                else
                {
                    if (p.thumbnail == "default")
                    {
                        p.thumbnail = "/Assets/Externallink.png";
                    }

                }
            }
            List<Subreddit> subs = await _consumer.GetSubscribedSubredditsAsync();
            UserIsSubscribed = !subs.Contains(_Subreddit);
            PostsReadyEvent.Invoke();
        }

        public async Task SubscribeToSubreddit()
        {
            UserIsSubscribed = !UserIsSubscribed;
            await _consumer.SubscribeToSubreddit(_Subreddit, !UserIsSubscribed);
        }

        public async Task Initialize()
        {
            if (await Authorize() != null)
            {
                //GeneratePosts();
            }
            else
            {
                if (firstTime == true)
                {
                    firstTime = false;
                }
                else
                {
                    await Initialize();
                }
            }
        }
    }
}