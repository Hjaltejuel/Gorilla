using Entities.RedditEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITEST.ViewModel
{
    public class CreatePostPageViewModel
    {
        private readonly IRedditAPIConsumer _consumer;
        private readonly Subreddit _currentSubreddit;

        public delegate void PostSent();
        public event PostSent PostSentEvent;

        public CreatePostPageViewModel(IRedditAPIConsumer consumer, Subreddit subreddit)
        {
            _consumer = consumer;
            _currentSubreddit = subreddit;
        }

        public async Task CreateNewPostAsync(string title, string body = "")
        {
            await _consumer.CreatePostAsync(_currentSubreddit, title, "self", body);
            PostSentEvent.Invoke();
        }
    }
}
