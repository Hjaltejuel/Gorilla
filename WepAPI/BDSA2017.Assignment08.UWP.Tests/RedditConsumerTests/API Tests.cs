using Entities.RedditEntities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using UI.Lib.Model.RedditRestInterfaces;
using UI.Lib.Model.RedditRepositories;

namespace RedditAPIConsumer.Tests
{
    public class UnitTest1
    {
        private readonly IRedditApiConsumer _rcc;
        public UnitTest1()
        {
            _rcc = new RedditConsumerController();
        }

        [Fact(DisplayName = "Get comments on post")]
        public async Task Get_Post_And_Comments_Success()
        {

            var post = await _rcc.GetPostAndCommentsByIdAsync("7gukik");
            Assert.Equal("Carpet cleaning", post.Item2.title);
        }

        [Fact(DisplayName = "Get value on post")]
        public async Task Get_Value_Post_And_Comments_Success()
        {
            var post = await _rcc.GetPostAndCommentsByIdAsync("7gukik");
            Assert.NotNull(post.Item2.title);
        }


        [Fact(DisplayName = "Get posts from user")]
        public async Task Get_Posts_From_User()
        {
            var list = await _rcc.GetUserPosts("n0oah");
            Assert.Equal("n0oah", list.Item2[0].author);
        }

        [Fact(DisplayName = "Get value from user")]
        public async Task Get_Value_From_User()
        {
            var list = await _rcc.GetUserPosts("n0oah");
            Assert.NotNull(list.Item2[0].author);
        }

        [Fact(DisplayName = "Get comments from user")]
        public async Task Get_Comments_From_User()
        {
            var list = await _rcc.GetUserComments("n0oah");
            Assert.Equal("n0oah", list.Item2[0].author);
        }

        [Fact(DisplayName = "Get comments value from user")]
        public async Task Get_Comments_value_From_User()
        {
            var list = await _rcc.GetUserComments("n0oah");
            Assert.NotNull(list.Item2[0].author);
        }

        [Fact(DisplayName = "Get values from Subreddit")]
        public async Task Test_Get_Posts_From_Subreddit()
        {
            var S = await _rcc.GetSubredditAsync("AskReddit");
            Assert.NotNull(S.Item2.display_name);
        }

        [Fact(DisplayName = "Get posts from Chess")]
        public async Task Test_Get_Posts_From_Subreddit_Success2()
        {
            var S = await _rcc.GetSubredditAsync("chess");
            Assert.Equal("chess", S.Item2.display_name);
        }

        [Fact(DisplayName = "Get comments on post 2")]
        public async Task Get_Post_And_Comments_2_Success()
        {
            var post = await _rcc.GetPostAndCommentsByIdAsync("7i0s1o");
            Assert.Equal("What's the fastest way you've seen someone improve their life?", post.Item2.title);
        }

        [Fact(DisplayName = "Get account value")]
        public async Task Get_Account_Values()
        {
            var user = await _rcc.GetAccountDetailsAsync();
            Assert.NotNull(user.Item2.name);
        }

        [Fact(DisplayName = "Get account details")]
        public async Task Get_Account_Details()
        {
            var user = await _rcc.GetAccountDetailsAsync();
            Assert.Equal("YAzEEEEEEEES", user.Item2.name);
        }

        [Fact(DisplayName = "Post a comment")]
        public async Task Post_Comment()
        {
            var pretendPost = new Post()
            {
                name = "t3_6q7512"
            };
            var reponse = await _rcc.CreateCommentAsync(pretendPost, "YoYoYo!");
            Assert.Equal(HttpStatusCode.OK, reponse.Item1);
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
            var subscribed = subscribedSubreddits.Item2.FirstOrDefault(e => e.name.Equals("t5_2qgzy"));

            var pretendSubreddit = new Subreddit()
            {
                name = "t5_2qgzy"
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

            Assert.Equal(typeof(Subreddit), subreddits.Item2[0].GetType());
        }

        [Fact(DisplayName = "Get value from subscriptions")]
        public async Task Get_Any_Subreddit_Subscription_Value()
        {
            var subreddits = await _rcc.GetSubscribedSubredditsAsync();

            Assert.NotNull(subreddits.Item2[0]);
        }

        [Fact(DisplayName = "Create post")]
        public async Task Create_Post()
        {
            var subreddits = await _rcc.GetSubscribedSubredditsAsync();

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
            var list = await _rcc.GetMoreComments("t3_7gukik", sarray, 20);

            Assert.Equal(typeof(Comment), list.Item2[0].GetType());
        }


        [Fact(DisplayName = "Get any children value")]
        public async Task Get_Value_More_Children()
        {
            string[] sarray = new string[] {
                                                                                        "dqm5e9x",
                                                                                        "dqm5wuf",

                                                                                    };
            var list = await _rcc.GetMoreComments("t3_7gukik", sarray, 20);

            Assert.NotNull(list.Item2[0]);
        }

        [Fact(DisplayName = "Get response from home page content call")]
        public async Task Get_Value_From_GetHomePageContent()
        {
            (HttpStatusCode, ObservableCollection<Post>) touple = await _rcc.GetHomePageContent();

            Assert.NotNull(touple.Item2);
        }

    }
}