using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication2.Controllers;
using WebApplication2.Models;

namespace RedditAPIConsumer.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async System.Threading.Tasks.Task Test_JSON_Async()
        {
            RedditConsumerController rcc = new RedditConsumerController();
            Post p = await rcc.GetPostAsync("7gukik");

            Assert.IsNotNull(p);
        }
    }
}
