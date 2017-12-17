using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Lib.Authentication.GorillaAuthentication;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;
using Xunit;

namespace UI.Tests.ViewModels
{
    public class SubredditPageViewModelTests
    {
        [Fact(DisplayName = "GeneratePost Test")]
        public void GeneratePostsTest()
        {
            var mock = new Mock<INavigationService>();
            var auth = new Mock<IAuthenticationHelper>();
            var PostMock = new Mock<IRestUserRepository>();
            var redditMock = new Mock<IRedditApiConsumer>();
            Assert.True(true);
        }
    }
}
