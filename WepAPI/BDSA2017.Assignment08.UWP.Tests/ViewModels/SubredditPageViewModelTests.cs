using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using Moq;
using UI.Lib.Authentication.GorillaAuthentication;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;
using UI.Lib.ViewModel;
using Xunit;
using Entities.RedditEntities;
using Entities.GorillaEntities;
using Subreddit = Entities.RedditEntities.Subreddit;

namespace UI.Test.ViewModels
{
    public class SubredditPageViewModelTests
    {
        private readonly Mock<INavigationService> _navigationServiceMock;
        private readonly Mock<IAuthenticationHelper> _authenticationHelperMock;
        private readonly Mock<IRestUserPreferenceRepository> _restRepositoryMock;
        private readonly Mock<IRedditApiConsumer> _redditApiConsumerMock;
        private readonly Mock<IUserHandler> _userHandlerMock;
        private readonly Mock<IRestSubredditRepository> _subredditRepositoryMock;

        private SubredditPageViewModel _vm;
        public SubredditPageViewModelTests()
        {
            _navigationServiceMock = new Mock<INavigationService>();
            _authenticationHelperMock = new Mock<IAuthenticationHelper>();
            _restRepositoryMock = new Mock<IRestUserPreferenceRepository>();
            _redditApiConsumerMock = new Mock<IRedditApiConsumer>();
            _userHandlerMock = new Mock<IUserHandler>();
            _subredditRepositoryMock = new Mock<IRestSubredditRepository>();
            _vm = new SubredditPageViewModel(_authenticationHelperMock.Object, _navigationServiceMock.Object, _redditApiConsumerMock.Object, _restRepositoryMock.Object, _userHandlerMock.Object, _subredditRepositoryMock.Object);
            _vm._Subreddit = new Subreddit() { name = "Pubg" };
        }
        [Fact(DisplayName = "GeneratePost Test given subredditName PUBG sets _Subreddit with display name pubg")]
        public async void GeneratePostsTest()
        {
            //Arrange
            var returnResult = Task.FromResult((HttpStatusCode.OK, new Entities.RedditEntities.Subreddit() {
                    display_name = "Pubg",
                    posts = new ObservableCollection<Entities.RedditEntities.Post>() { new Entities.RedditEntities.Post() { title = "GOTY"} }
            }));
            _redditApiConsumerMock.Setup(o => o.GetSubredditPostsAsync(It.IsAny<Entities.RedditEntities.Subreddit>(), "hot"))
                                    .Returns(returnResult);

            //Act
            await _vm.GeneratePosts("hot");

            //Assert
            var expected = "Pubg";
            var actual = _vm._Subreddit.display_name;
            Assert.Equal(expected, actual);
            Assert.NotEmpty(_vm._Subreddit.posts);
        }

        [Fact(DisplayName = "GeneratePost Test given subredditName PUBG sets _Subreddit.display_name is null")]
        public async void GeneratePostsTestWhenSubredditBecomesNull()
        {
            //Arrange
            var returnResult = Task.FromResult((HttpStatusCode.OK, new Entities.RedditEntities.Subreddit()
            {
                display_name = null
            }));
            _redditApiConsumerMock.Setup(o => o.GetSubredditPostsAsync(It.IsAny<Entities.RedditEntities.Subreddit>(), "hot"))
                                     .Returns(returnResult);

            //Act
            await _vm.GeneratePosts("hot");

            //Assert
            var actual = _vm._Subreddit.display_name;
            Assert.Null(actual);
        }

        [Fact(DisplayName = "GeneratePost Test given no Sortparameter -> sorts by hot")]
        public async void GeneratePostsTestSortsByHotGivenNoSortParameter()
        {
            //Arrange
            _vm._Subreddit = new Entities.RedditEntities.Subreddit() { display_name = "Pubg" };

            //Act
            await _vm.GeneratePosts("hot");

            //Assert
            _redditApiConsumerMock.Verify(o => o.GetSubredditPostsAsync(It.IsAny<Entities.RedditEntities.Subreddit>(), "hot"));
        }

        [Fact(DisplayName = "SubscribeToSubreddit Test where user is not already subscribed")]
        public async void SubscribeToSubredditTestNotAlreadySubscribed()
        {
            //Arrange
            _vm.UserIsSubscribed = false;
            _vm._Subreddit = new Entities.RedditEntities.Subreddit() { display_name = "Pubg" };
            var userToReturn = new Entities.RedditEntities.User() { name = "UserOne" };
            _userHandlerMock.Setup(o => o.GetUser())
                                    .Returns(userToReturn);

            //Act
            await _vm.SubscribeToSubreddit();

            //Assert
            _restRepositoryMock.Verify(o => o.UpdateAsync(It.IsAny<UserPreference>()), Times.Once);
            Assert.True(_vm.UserIsSubscribed);
        }

        [Fact(DisplayName = "SubscribeToSubreddit Test where user is already subscribed")]
        public async void SubscribeToSubredditTestAlreadySubscribed()
        {
            //Arrange
            _vm.UserIsSubscribed = true;
            _vm._Subreddit = new Entities.RedditEntities.Subreddit() { display_name = "Pubg" };
            var userToReturn = new Entities.RedditEntities.User() { name = "UserOne" };
            _userHandlerMock.Setup(o => o.GetUser())
                                    .Returns(userToReturn);
            //Act
            await _vm.SubscribeToSubreddit();

            //Assert
            _restRepositoryMock.Verify(o => o.UpdateAsync(It.IsAny<UserPreference>()), Times.Once);
            Assert.False(_vm.UserIsSubscribed);
        }

        [Fact(DisplayName = "SortBy Test where it sorts by new")]
        public async void SortByTest()
        {
            //Arrange
            var selectedSort = "new";
            _vm._Subreddit = new Entities.RedditEntities.Subreddit() { };
            //Act
            _vm.SortBy(selectedSort);

            //Assert
            _redditApiConsumerMock.Verify(o => o.GetSubredditPostsAsync(It.IsAny<Entities.RedditEntities.Subreddit>(), "new"));
        }
    }
}
