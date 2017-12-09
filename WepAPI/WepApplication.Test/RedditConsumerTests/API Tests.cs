using Entities.RedditEntities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using Xunit;

namespace RedditAPIConsumer.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test_JSON_Async()
        {
            IRedditAPIConsumer rcc = new RedditConsumerController();
            Post post = await rcc.GetPostAndCommentsByIdAsync("7gukik");
            Assert.Equal("Carpet cleaning", post.title);
        }

        [Fact]
        public async Task Test_Get_Posts_From_Subreddit()
        {
            IRedditAPIConsumer rcc = new RedditConsumerController();
            Subreddit S = await rcc.GetSubredditAsync("AskReddit");

            Assert.Equal("AskReddit", S.display_name);
        }
        [Fact]
        public async Task TestDeserialization()
        {

            IRedditAPIConsumer rcc = new RedditConsumerController();
            Post post = await rcc.GetPostAndCommentsByIdAsync("7i0s1o");

            Assert.Equal("What's the fastest way you've seen someone improve their life?", post.title);

        }
    }
}
