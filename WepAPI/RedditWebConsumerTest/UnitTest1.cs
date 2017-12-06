using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Newtonsoft.Json;
using WebApplication2.Models;
using WebApplication2.Controllers;

namespace RedditWebConsumerTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async void Test_If_Converter_WorksAsync()
        {
            RedditConsumerController rcc = new RedditConsumerController();
            string json = await rcc.GetPostAsync("7gukik")
            var dict = JsonConvert.DeserializeObject<Dictionary<string, Comment>>(json, new NodeConverter());
        }
    }
}
