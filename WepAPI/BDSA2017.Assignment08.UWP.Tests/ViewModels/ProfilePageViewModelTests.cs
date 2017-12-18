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
            _userHandler.Setup(o => o.GetUserName()).Returns(username);
            _restPostRepository.Setup(o => o.ReadAsync(username)).Returns(Task.FromResult(returnList));
            //Act
            await _profilePageViewModel.GetVisistedPosts();

            //Assert
            _userHandler.Verify(v => v.GetUserName(), Times.Once);
            _restPostRepository.Verify(v => v.ReadAsync(username), Times.Once);
            _redditApiConsumer.Verify(v => v.GetPostsByIdAsync(It.IsAny<string>()), Times.Once);
        }

    }
}
