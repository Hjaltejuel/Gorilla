using Moq;
using UITEST.Authentication.GorillaAuthentication;
using UITEST.Model;
using UITEST.Model.GorillaRestInterfaces;
using UITEST.Model.RedditRestInterfaces;
using UITEST.ViewModel;
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
            MainPageViewModel view = new MainPageViewModel(auth.Object, mock.Object, redditMock.Object, PostMock.Object);
            Assert.True(true);
            
        }

    }
}
