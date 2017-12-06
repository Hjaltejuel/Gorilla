using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication2.Controllers;

namespace RedditConsumeTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async void TestMethod1Async()
        {
            RedditConsumerController rcc = new RedditConsumerController();
            await rcc.GetPostAsync("7gukik");
        }
    }
}
