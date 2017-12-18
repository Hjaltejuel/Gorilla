using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using Entities.RedditEntities;
using Moq;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;
using UI.Lib.ViewModel;
using Xunit;

namespace UI.Test.ViewModels
{
    public class ProfilePageViewModelTests
    {
        private readonly Mock<IRestPostRepository> _restPostRepository;
        private readonly Mock<INavigationService> _navigationService;
        private readonly Mock<IRedditApiConsumer> _redditApiConsumer;
        private readonly Mock<IRestUserRepository> _restUserPreferenceRepository;
        private readonly Mock<IUserHandler> _userHandler;
        private readonly ProfilePageViewModel _profilePageViewModel;

        public ProfilePageViewModelTests()
        {
            _restPostRepository = new Mock<IRestPostRepository>();
            _navigationService = new Mock<INavigationService>();
            _redditApiConsumer = new Mock<IRedditApiConsumer>();
            _restUserPreferenceRepository = new Mock<IRestUserRepository>();
            _userHandler = new Mock<IUserHandler>();

            _profilePageViewModel = new ProfilePageViewModel(
                _navigationService.Object,
                _restUserPreferenceRepository.Object,
                _redditApiConsumer.Object,
                _restPostRepository.Object,
                _userHandler.Object
            );
        }

        [Fact(DisplayName = "Profile page - GetVisistedPosts Test")]
        public async void GetVisistedPosts_Test()
        {
            //Arrange
            var username = "Username";
            IReadOnlyCollection<Entities.GorillaEntities.Post> returnList = new List<Entities.GorillaEntities.Post>()
            {
                new Entities.GorillaEntities.Post(){ Id="A"},
                new Entities.GorillaEntities.Post(){ Id="B"},
                new Entities.GorillaEntities.Post(){ Id="C"}
            };
            var aggregatedIdsString = ",t3_A,t3_B,t3_C";
            _userHandler.Setup(o => o.GetUserName()).Returns(username);
            _restPostRepository.Setup(o => o.ReadAsync(username)).Returns(Task.FromResult(returnList));
            //Act
            await _profilePageViewModel.GetVisistedPosts();

            //Assert
            _userHandler.Verify(v => v.GetUserName(), Times.Once);
            _restPostRepository.Verify(v => v.ReadAsync(username), Times.Once);
            _redditApiConsumer.Verify(v => v.GetPostsByIdAsync(aggregatedIdsString), Times.Once);
        }

        [Fact(DisplayName = "Profile page - GetCurrentProfile() (No user found)")]
        public async void GetCurrentUser_Test_Fail_No_User()
        {
            //Arrange
            var user = (User)null;
            _userHandler.Setup(o => o.GetUser()).Returns(user);

            //Act
            await _profilePageViewModel.GetCurrentProfile();

            //Assert
            _redditApiConsumer.Verify(v => v.GetSubscribedSubredditsAsync(), Times.Never);
            _redditApiConsumer.Verify(v => v.GetUserPosts(It.IsAny<string>()), Times.Never);
            _redditApiConsumer.Verify(v => v.GetUserComments(It.IsAny<string>()), Times.Never);
        }

        [Fact(DisplayName = "Profile page - GetCurrentProfile() (No Subscriptions, posts or comments)")]
        public async void GetCurrentUser_Test_Fail_No_Subscriptions_Posts_Comments()
        {
            //Arrange
            var user = new User() { name = "Username" };

            var subcriptionResult = Task.FromResult((HttpStatusCode.BadRequest, (List<Subreddit>)null));
            var postsResult = Task.FromResult((HttpStatusCode.BadRequest, (ObservableCollection<Post>)null));
            var commentsResult = Task.FromResult((HttpStatusCode.BadRequest, (ObservableCollection<Comment>)null));

            _userHandler.Setup(o => o.GetUser()).Returns(user);

            _redditApiConsumer.Setup(o => o.GetSubscribedSubredditsAsync()).Returns(subcriptionResult);
            _redditApiConsumer.Setup(o => o.GetUserPosts(user.name)).Returns(postsResult);
            _redditApiConsumer.Setup(o => o.GetUserComments(user.name)).Returns(commentsResult);

            //Act
            await _profilePageViewModel.GetCurrentProfile();

            //Assert
            _redditApiConsumer.Verify(v => v.GetSubscribedSubredditsAsync(), Times.Once);
            _redditApiConsumer.Verify(v => v.GetUserPosts(user.name), Times.Once);
            _redditApiConsumer.Verify(v => v.GetUserComments(user.name), Times.Once);
        }


        [Fact(DisplayName = "Profile page - GetCurrentProfile() (Sucess)")]
        public async void GetCurrentUser_Test_Success()
        {
            //Arrange
            var user = new User() { name = "Username" };

            var subcriptionResult = Task.FromResult((HttpStatusCode.BadRequest, new List<Subreddit>()));
            var postsResult = Task.FromResult((HttpStatusCode.BadRequest, new ObservableCollection<Post>()));
            var commentsResult = Task.FromResult((HttpStatusCode.BadRequest, new ObservableCollection<Comment>()));

            _userHandler.Setup(o => o.GetUser()).Returns(user);

            _redditApiConsumer.Setup(o => o.GetSubscribedSubredditsAsync()).Returns(subcriptionResult);
            _redditApiConsumer.Setup(o => o.GetUserPosts(user.name)).Returns(postsResult);
            _redditApiConsumer.Setup(o => o.GetUserComments(user.name)).Returns(commentsResult);

            //Act
            await _profilePageViewModel.GetCurrentProfile();

            //Assert
            _redditApiConsumer.Verify(v => v.GetSubscribedSubredditsAsync(), Times.Once);
            _redditApiConsumer.Verify(v => v.GetUserPosts(user.name), Times.Once);
            _redditApiConsumer.Verify(v => v.GetUserComments(user.name), Times.Once);
        }
    }
}
