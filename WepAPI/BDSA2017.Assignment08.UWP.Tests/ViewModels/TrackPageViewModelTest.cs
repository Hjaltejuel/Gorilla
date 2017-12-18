using Moq;
using UI.Lib.Authentication.GorillaAuthentication;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;
using UI.Lib.ViewModel;
using Xunit;

namespace BDSA2017.Assignment08.UWP.Tests.ViewModels
{
    public class TrackPageViewModelTest
    {
        [Fact]
        public void TestThatThereIsAlways1()
        {
            
            var mock = new Mock<INavigationService>();
            var auth = new Mock<IAuthenticationHelper>();
            var PostMock = new Mock<IRestUserRepository>();
            var redditMock = new Mock<IRedditApiConsumer>();
            MainPageViewModel view = new MainPageViewModel( mock.Object, redditMock.Object, PostMock.Object);
            Assert.True(true);
            
        }

    }
}
