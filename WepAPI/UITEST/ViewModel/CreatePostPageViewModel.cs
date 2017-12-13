using Entities.RedditEntities;
using Gorilla.AuthenticationGorillaAPI;
using Gorilla.Model;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace UITEST.ViewModel
{
    public class CreatePostPageViewModel : BaseViewModel
    {
        private readonly IRedditAPIConsumer _consumer;
<<<<<<< HEAD
        private readonly Subreddit _currentSubreddit;
=======
        private Subreddit CurrentSubreddit;
>>>>>>> 9366c7b5baff501d8a5edfeeb730de9c989ea2d2

        public delegate void PostSent();
        public event PostSent SentSuccesfulEvent;
        public event PostSent SentUnsuccesfulEvent;

        public Subreddit currentSubreddit
        {
            get { return CurrentSubreddit; }
            set {
                if(value != CurrentSubreddit)
                {
                    CurrentSubreddit = value;
                    OnPropertyChanged("CurrentSubreddit");
                }
            }
        }


        public CreatePostPageViewModel(IAuthenticationHelper helper, INavigationService service, IRedditAPIConsumer consumer) : base(service)
        {
            _consumer = consumer;
<<<<<<< HEAD
            _currentSubreddit = subreddit;
=======
            _helper = helper;
>>>>>>> 9366c7b5baff501d8a5edfeeb730de9c989ea2d2
        }

        public async Task CreateNewPostAsync(string title, string body = "")
        {
<<<<<<< HEAD
            await _consumer.CreatePostAsync(_currentSubreddit, title, "self", body);
            PostSentEvent.Invoke();
=======
            var response = await _consumer.CreatePostAsync(CurrentSubreddit, title, "self", body);
            if (response.Item1 == HttpStatusCode.OK)
            {
                SentSuccesfulEvent.Invoke();
            }
            else
            {
                SentUnsuccesfulEvent.Invoke();
            }
>>>>>>> 9366c7b5baff501d8a5edfeeb730de9c989ea2d2
        }
    }
}
