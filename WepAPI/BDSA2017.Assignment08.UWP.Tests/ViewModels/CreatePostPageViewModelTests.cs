using Entities.RedditEntities;
using Moq;
using UI.Lib.Authentication.GorillaAuthentication;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;
using UI.Lib.ViewModel;
using Xunit;

namespace BDSA2017.Assignment08.UWP.Tests.GorillaRepoTests
{
    public class CreatePostPageViewModelTests
    {
        private readonly Mock<INavigationService> service;
        private readonly Mock<IRestUserPreferenceRepository> repository;
        private readonly Mock<IRedditApiConsumer> consumer;
        private readonly Mock<IUserHandler> userHandler;

        private readonly CreatePostPageViewModel _createPostPageViewModel;
        public CreatePostPageViewModelTests()
        {
            service = new Mock<INavigationService>();
            repository = new Mock<IRestUserPreferenceRepository>();
            consumer = new Mock<IRedditApiConsumer>();
            userHandler = new Mock<IUserHandler>();
            _createPostPageViewModel = new CreatePostPageViewModel(service.Object, consumer.Object, repository.Object, userHandler.Object);
        }

        [Fact(DisplayName = "CreateNewPost Test With a title")]
        public async void CreateNewPostAsyncWithATitle()
        {
            //Arrange
            string title = "El title";
            _createPostPageViewModel.LoadingRingOnOf += () => { };

            //Act
            await _createPostPageViewModel.CreateNewPostAsync(title);

            //Assert
            consumer.Verify(o => o.CreatePostAsync(It.IsAny<Subreddit>(), title, "self", "", ""), Times.Once);
        }

        [Fact(DisplayName = "CreateNewPost Test With an empty title")]
        public async void CreateNewPostAsyncWithAnEmptyTitle()
        {
            //Arrange
            string title = "";

            //Act
            await _createPostPageViewModel.CreateNewPostAsync(title);

            //Assert
            consumer.Verify(o => o.CreatePostAsync(It.IsAny<Subreddit>(), title, "self", "", ""), Times.Never);
        }
    }
}