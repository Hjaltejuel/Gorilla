using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Entities.RedditEntities;
using UI.Lib.Model.RedditRestInterfaces;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using UI.Lib.Authentication;
using UI.Lib.Model;
using UI.Lib.Model.RedditRepositories;

namespace UI.Test.RedditConsumerTests
{
    public class RedditApiConsumerTests
    {
        private IRedditApiConsumer _consumer;
        private IUserHandler _userHandler;
        private IRedditAuthHandler _authHandler;
        public RedditApiConsumerTests()
        {
            //Login data
            ApplicationDataContainer _appSettings = ApplicationData.Current.RoamingSettings;
            _appSettings.Values["reddit_refresh_token"] = "51999737725-Fi9-iKz5EU-4-gW5luSquB6ysyY";

            _consumer = new RedditConsumerController();
            _userHandler = new UserHandler();
            _authHandler = new RedditAuthHandler(_consumer, _userHandler);
            _authHandler.BeginAuth();
            _consumer.Authenticate(_authHandler);
        }

        [Fact(DisplayName = "Get comments on post")]
        public async Task Get_Post_And_Comments_Success()
        {
            var post = await _consumer.GetPostAndCommentsByIdAsync("7gukik");
            Assert.Equal("Carpet cleaning", post.Item2.title);
        }

        [Fact(DisplayName = "Get value on post")]
        public async Task Get_Value_Post_And_Comments_Success()
        {
            var post = await _consumer.GetPostAndCommentsByIdAsync("7gukik");
            Assert.NotNull(post.Item2.title);
        }

        [Fact(DisplayName = "Get posts from user")]
        public async Task Get_Posts_From_User()
        {
            var list = await _consumer.GetUserPosts("n0oah");
            Assert.Equal("n0oah", list.Item2[0].author);
        }

        [Fact(DisplayName = "Get value from user")]
        public async Task Get_Value_From_User()
        {
            var list = await _consumer.GetUserPosts("n0oah");
            Assert.NotNull(list.Item2[0].author);
        }

        [Fact(DisplayName = "Get comments from user")]
        public async Task Get_Comments_From_User()
        {
            var list = await _consumer.GetUserComments("n0oah");
            Assert.Equal("n0oah", list.Item2[0].author);
        }

        [Fact(DisplayName = "Get comments value from user")]
        public async Task Get_Comments_value_From_User()
        {
            var list = await _consumer.GetUserComments("n0oah");
            Assert.NotNull(list.Item2[0].author);
        }

        [Fact(DisplayName = "Get values from Subreddit")]
        public async Task Test_Get_Posts_From_Subreddit()
        {
            var S = await _consumer.GetSubredditAsync("AskReddit");
            Assert.NotNull(S.Item2.display_name);
        }

        [Fact(DisplayName = "Get posts from Chess")]
        public async Task Test_Get_Posts_From_Subreddit_Success2()
        {
            var S = await _consumer.GetSubredditAsync("chess");
            Assert.Equal("chess", S.Item2.display_name);
        }

        [Fact(DisplayName = "Get comments on post 2")]
        public async Task Get_Post_And_Comments_2_Success()
        {
            var post = await _consumer.GetPostAndCommentsByIdAsync("7i0s1o");
            Assert.Equal("What's the fastest way you've seen someone improve their life?", post.Item2.title);
        }

        [Fact(DisplayName = "Get account details")]
        public async Task Get_Account_Details()
        {
            var user = await _consumer.GetAccountDetailsAsync();
            Assert.Equal("YAzEEEEEEEES", user.Item2.name);
        }

        [Fact(DisplayName = "Post a comment")]
        public async Task Post_Comment()
        {
            var pretendPost = new Post()
            {
                name = "t3_6q7512"
            };
            var reponse = await _consumer.CreateCommentAsync(pretendPost, "YoYoYo!");
            Assert.Equal(HttpStatusCode.OK, reponse.Item1);
        }

        [Fact(DisplayName = "Cast a vote on a post")]
        public async Task Cast_Vote()
        {
            var pretendPost = new Post()
            {
                name = "t3_6q7512"
            };
            var response = await _consumer.VoteAsync(pretendPost, 1);

            Assert.Equal((HttpStatusCode.OK, "Vote succesful!"), response);
        }
        
        [Fact(DisplayName = "Subscribe/Unsubscribe to a subreddit")]
        public async Task Subscribe_Test()
        {
            var subscribedSubreddits = await _consumer.GetSubscribedSubredditsAsync();
            var subscribed = subscribedSubreddits.Item2.FirstOrDefault(e => e.name.Equals("t5_2qgzy"));

            var pretendSubreddit = new Subreddit()
            {
                name = "t5_2qgzy"
            };

            if (subscribed == null)
            {
                var response = await _consumer.SubscribeToSubreddit(pretendSubreddit, true);
                Assert.Equal((HttpStatusCode.OK, "Subcribe successful!"), response);
            }
            else
            {
                var response = await _consumer.SubscribeToSubreddit(pretendSubreddit, false);
                Assert.Equal((HttpStatusCode.OK, "Subcribe successful!"), response);
            }
        }

        [Fact(DisplayName = "Get subscriptions")]
        public async Task Get_My_Subreddit_Subscriptions()
        {
            var subreddits = await _consumer.GetSubscribedSubredditsAsync();

            Assert.Equal(typeof(Subreddit), subreddits.Item2[0].GetType());
        }

        [Fact(DisplayName = "Get value from subscriptions")]
        public async Task Get_Any_Subreddit_Subscription_Value()
        {
            var subreddits = await _consumer.GetSubscribedSubredditsAsync();

            Assert.NotNull(subreddits.Item2[0]);
        }

        [Fact(DisplayName = "Create post")]
        public async Task Create_Post()
        {
            var subreddits = await _consumer.GetSubscribedSubredditsAsync();

            Assert.Equal(typeof(Subreddit), subreddits.Item2[0].GetType());
        }


        [Fact(DisplayName = "Get more children")]
        public async Task Get_More_Children()
        {
            string[] sarray = new string[] {
                                                                                        "dqm5e9x",
                                                                                        "dqm5wuf",
                                                                                        "dqmfdlb",
                                                                                        "dqn4xao",
                                                                                        "dqmafpv",
                                                                                        "dqmf6vp",
                                                                                        "dqn3ct5",
                                                                                        "dqm4cyr",
                                                                                        "dqn894f",
                                                                                        "dqma8mr",
                                                                                        "dqm360r",
                                                                                        "dqmccdv",
                                                                                        "dqm4bm1",
                                                                                        "dqn0nyi",
                                                                                        "dqm59xr",
                                                                                        "dqoj6jj",
                                                                                        "dqmjzoh",
                                                                                        "dqm9qaq",
                                                                                        "dqmcin9",
                                                                                        "dqmg1v2",
                                                                                        "dqmbla0",
                                                                                        "dqmopks"
                                                                                    };
            var list = await _consumer.GetMoreComments("t3_7gukik", sarray, 20);

            Assert.Equal(typeof(Comment), list.Item2[0].GetType());
        }


        [Fact(DisplayName = "Get any children value")]
        public async Task Get_Value_More_Children()
        {
            string[] sarray = new string[] {
                                                                                        "dqm5e9x",
                                                                                        "dqm5wuf",

                                                                                    };
            var list = await _consumer.GetMoreComments("t3_7gukik", sarray, 20);

            Assert.NotNull(list.Item2[0]);
        }

        [Fact(DisplayName = "Get response from home page content call")]
        public async Task Get_Value_From_GetHomePageContent()
        {
            (HttpStatusCode, ObservableCollection<Post>) touple = await _consumer.GetHomePageContent();

            Assert.NotNull(touple.Item2);
        }

    }
}