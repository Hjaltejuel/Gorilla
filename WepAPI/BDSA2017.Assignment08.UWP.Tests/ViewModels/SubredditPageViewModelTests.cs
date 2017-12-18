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

namespace UI.Test.ViewModels
{
    public class SubredditPageViewModelTests
    {
        private readonly Mock<INavigationService> service;
        private readonly Mock<IAuthenticationHelper> helper;
        private readonly Mock<IRestUserPreferenceRepository> repository;
        private readonly Mock<IRedditApiConsumer> consumer;
        private readonly Mock<IUserHandler> _userHandler;

        public SubredditPageViewModelTests()
        {
            service = new Mock<INavigationService>();
            helper = new Mock<IAuthenticationHelper>();
            repository = new Mock<IRestUserPreferenceRepository>();
            consumer = new Mock<IRedditApiConsumer>();
            _userHandler = new Mock<IUserHandler>();
        }
        [Fact(DisplayName = "GeneratePost Test given subredditName PUBG sets _Subreddit with display name pubg")]
        public async void GeneratePostsTest()
        {
            //Arrange
            var returnResult = Task.FromResult((HttpStatusCode.OK, new Entities.RedditEntities.Subreddit() {
                    display_name = "Pubg",
                    posts = new ObservableCollection<Entities.RedditEntities.Post>() { new Entities.RedditEntities.Post() { title = "GOTY"} }
            }));
            consumer.Setup(o => o.GetSubredditPostsAsync(It.IsAny<Entities.RedditEntities.Subreddit>(), "hot"))
                                    .Returns(returnResult);
            var vm = new SubredditPageViewModel(helper.Object, service.Object, consumer.Object, repository.Object, _userHandler.Object);

            //Act
            await vm.GeneratePosts("Pubg");

            //Assert
            var expected = "Pubg";
            var actual = vm._Subreddit.display_name;
            Assert.Equal(expected, actual);
            Assert.NotEmpty(vm._Subreddit.posts);
        }

        [Fact(DisplayName = "GeneratePost Test given subredditName PUBG sets _Subreddit.display_name is null")]
        public async void GeneratePostsTestWhenSubredditBecomesNull()
        {
            //Arrange
            var returnResult = Task.FromResult((HttpStatusCode.OK, new Entities.RedditEntities.Subreddit()
            {
                display_name = null
            }));
            consumer.Setup(o => o.GetSubredditPostsAsync(It.IsAny<Entities.RedditEntities.Subreddit>(), "hot"))
                                     .Returns(returnResult);

            var vm = new SubredditPageViewModel(helper.Object, service.Object, consumer.Object, repository.Object, _userHandler.Object);

            //Act
            await vm.GeneratePosts("Pubg");

            //Assert
            var actual = vm._Subreddit.display_name;
            Assert.Null(actual);
        }

        [Fact(DisplayName = "GeneratePost Test given no Sortparameter -> sorts by hot")]
        public async void GeneratePostsTestSortsByHotGivenNoSortParameter()
        {
            //Arrange
            var vm = new SubredditPageViewModel(helper.Object, service.Object, consumer.Object, repository.Object, _userHandler.Object);
            vm._Subreddit = new Entities.RedditEntities.Subreddit() { display_name = "Pubg" };

            //Act
            await vm.GeneratePosts(vm._Subreddit.display_name);

            //Assert
            consumer.Verify(o => o.GetSubredditPostsAsync(It.IsAny<Entities.RedditEntities.Subreddit>(), "hot"));
        }

        [Fact(DisplayName = "SubscribeToSubreddit Test where user is not already subscribed")]
        public async void SubscribeToSubredditTestNotAlreadySubscribed()
        {
            //Arrange
            var vm = new SubredditPageViewModel(helper.Object, service.Object, consumer.Object, repository.Object, _userHandler.Object);
            vm.UserIsSubscribed = false;
            vm._Subreddit = new Entities.RedditEntities.Subreddit() { display_name = "Pubg" };
            var userToReturn = new Entities.RedditEntities.User() { name = "UserOne" };
            _userHandler.Setup(o => o.GetUser())
                                    .Returns(userToReturn);

            //Act
            await vm.SubscribeToSubreddit();

            //Assert
            repository.Verify(o => o.UpdateAsync(It.IsAny<UserPreference>()), Times.Once);
            Assert.True(vm.UserIsSubscribed);
        }

        [Fact(DisplayName = "SubscribeToSubreddit Test where user is already subscribed")]
        public async void SubscribeToSubredditTestAlreadySubscribed()
        {
            //Arrange
            var vm = new SubredditPageViewModel(helper.Object, service.Object, consumer.Object, repository.Object, _userHandler.Object);
            vm.UserIsSubscribed = true;
            vm._Subreddit = new Entities.RedditEntities.Subreddit() { display_name = "Pubg" };
            var userToReturn = new Entities.RedditEntities.User() { name = "UserOne" };
            _userHandler.Setup(o => o.GetUser())
                                    .Returns(userToReturn);

            //Act
            await vm.SubscribeToSubreddit();

            //Assert
            repository.Verify(o => o.UpdateAsync(It.IsAny<UserPreference>()), Times.Once);
            Assert.False(vm.UserIsSubscribed);
        }


        [Fact(DisplayName = "SortBy Test where it sorts by new")]
        public async void SortByTest()
        {
            //Arrange
            var vm = new SubredditPageViewModel(helper.Object, service.Object, consumer.Object, repository.Object, _userHandler.Object);
            vm.selectedSort = "new";
            vm._Subreddit = new Entities.RedditEntities.Subreddit() { };
            //Act
            vm.SortBy();

            //Assert
            consumer.Verify(o => o.GetSubredditPostsAsync(It.IsAny<Entities.RedditEntities.Subreddit>(), "new"));
        }
    }
}
