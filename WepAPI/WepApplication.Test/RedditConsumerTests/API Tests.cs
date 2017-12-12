using Entities.RedditEntities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication2.Models;
using Xunit;

namespace RedditAPIConsumer.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Get_Post_And_Comments_Success()
        {
            IRedditAPIConsumer rcc = new RedditConsumerController();
            Post post = await rcc.GetPostAndCommentsByIdAsync("7gukik");
            Assert.Equal("Carpet cleaning", post.title);
        }

        [Fact]
        public async Task Test_Get_Posts_From_Subreddit_Success()
        {
            IRedditAPIConsumer rcc = new RedditConsumerController();
            Subreddit S = await rcc.GetSubredditAsync("AskReddit");
            Assert.Equal("AskReddit", S.display_name);
        }
        [Fact]
        public async Task Get_Post_And_Comments_2_Success()
        {
            IRedditAPIConsumer rcc = new RedditConsumerController();
            Post post = await rcc.GetPostAndCommentsByIdAsync("7i0s1o");
            Assert.Equal("What's the fastest way you've seen someone improve their life?", post.title);
        }
        [Fact]
        public async Task Login_To_Reddit_Success()
        {
            IRedditAPIConsumer rcc = new RedditConsumerController();
            (HttpStatusCode, string) loginResponse = await rcc.LoginToReddit("YAzEEEEEEEES", "12341234");
            Assert.Equal((HttpStatusCode.OK, "Logged in successfully"), loginResponse);
        }

        [Fact]
        public async Task Login_To_Reddit_And_Get_Information()
        {
            IRedditAPIConsumer rcc = new RedditConsumerController();
            //(HttpStatusCode, string) loginResponse = await rcc.LoginToReddit("YAzEEEEEEEES", "12341234");

            User asd = await rcc.GetAccountDetails();
            string a = "";
        }

        [Fact]
        public async Task Post_Comment()
        {
            IRedditAPIConsumer rcc = new RedditConsumerController();
            //(HttpStatusCode, string) loginResponse = await rcc.LoginToReddit("YAzEEEEEEEES", "12341234");
            Post pretendPost = new Post()
            {
                name = "t3_6q7512"
            };
            var reponse = await rcc.PostComment(pretendPost, "YoYoYo!");
            var a = "";
        }

        [Fact]
        public async Task Cast_Vote()
        {
            IRedditAPIConsumer rcc = new RedditConsumerController();
            //(HttpStatusCode, string) loginResponse = await rcc.LoginToReddit("YAzEEEEEEEES", "12341234");
            Post pretendPost = new Post()
            {
                name = "t3_6q7512"
            };
            var reponse = await rcc.PostVoteAsync(pretendPost, 1);
            var a = "";
        }

        [Fact]
        public async Task Get_My_Subreddit_Subscriptions()
        {
            IRedditAPIConsumer rcc = new RedditConsumerController();
            //(HttpStatusCode, string) loginResponse = await rcc.LoginToReddit("YAzEEEEEEEES", "12341234");
            List<Subreddit> subreddits = await rcc.GetSubscribedSubreddits();
            
            Assert.Equal(typeof(Subreddit), subreddits[0].GetType());
        }
    }
}
