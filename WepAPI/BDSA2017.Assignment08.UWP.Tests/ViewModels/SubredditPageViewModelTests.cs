using Moq;
using UI.Lib.Authentication.GorillaAuthentication;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;
using Xunit;

namespace UI.Test.ViewModels
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
