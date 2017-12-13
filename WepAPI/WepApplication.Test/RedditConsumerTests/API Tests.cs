using Entities.RedditEntities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication2.Models;
using Xunit;

namespace RedditAPIConsumer.Tests
{
    public class UnitTest1
    {
        private readonly IRedditAPIConsumer _rcc;
        public UnitTest1()
        {
            _rcc = new RedditConsumerController();
            var authed = _rcc.RefreshTokenAsync().Result;
        }
        [Fact(DisplayName = "Get comments on post")]
        public async Task Get_Post_And_Comments_Success()
        {
            var post = await _rcc.GetPostAndCommentsByIdAsync("7gukik");
            Assert.Equal("Carpet cleaning", post.title);
        }

        [Fact(DisplayName = "Get posts from AskReddit")]
        public async Task Test_Get_Posts_From_Subreddit_Success()
        {
            var S = await _rcc.GetSubredditAsync("AskReddit");
            Assert.Equal("AskReddit", S.display_name);
        }

        [Fact(DisplayName = "Get comments on post 2")]
        public async Task Get_Post_And_Comments_2_Success()
        {
            var post = await _rcc.GetPostAndCommentsByIdAsync("7i0s1o");
            Assert.Equal("What's the fastest way you've seen someone improve their life?", post.title);
        }

        [Fact(DisplayName = "Get account details")]
        public async Task Get_Account_Details()
        {
            var user = await _rcc.GetAccountDetailsAsync();
            Assert.Equal("YAzEEEEEEEES", user.name);
        }

        [Fact(DisplayName = "Post a comment")]
        public async Task Post_Comment()
        {
            var pretendPost = new Post()
            {
                name = "t3_6q7512"
            };
            var reponse = await _rcc.CreateCommentAsync(pretendPost, "YoYoYo!");
            Assert.Equal((HttpStatusCode.OK, "Comment was posted!"), reponse);
        }

        [Fact(DisplayName = "Cast a vote on a post")]
        public async Task Cast_Vote()
        {
            var pretendPost = new Post()
            {
                name = "t3_6q7512"
            };
            var response = await _rcc.VoteAsync(pretendPost, 1);

            Assert.Equal((HttpStatusCode.OK, "Vote succesful!"), response);
        }


        [Fact(DisplayName = "Subscribe/Unsubscribe to a subreddit")]
        public async Task Subscribe_Test()
        {
            var subscribedSubreddits = await _rcc.GetSubscribedSubredditsAsync();
            var subscribed = subscribedSubreddits.FirstOrDefault(e => e.name.Equals("t5_2qgzy"));

            var pretendSubreddit = new Subreddit()
            {
                name = "t5_2qgzy" //Sports
            };

            if (subscribed == null)
            {
                var response = await _rcc.SubscribeToSubreddit(pretendSubreddit, true);
                Assert.Equal((HttpStatusCode.OK, "Subcribe successful!"), response);
            }
            else
            {
                var response = await _rcc.SubscribeToSubreddit(pretendSubreddit, false);
                Assert.Equal((HttpStatusCode.OK, "Subcribe successful!"), response);
            }
        }

        [Fact(DisplayName = "Get subscriptions")]
        public async Task Get_My_Subreddit_Subscriptions()
        {
            var subreddits = await _rcc.GetSubscribedSubredditsAsync();
            
            Assert.Equal(typeof(Subreddit), subreddits[0].GetType());
        }
    }
}
