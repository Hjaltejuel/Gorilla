using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Entities.RedditEntities;
using Moq;
using UI.Lib.Authentication.GorillaAuthentication;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;
using UI.Lib.ViewModel;
using Xunit;

namespace UI.Test.ViewModels
{
    public class MainPageViewModelTest
    {
        private readonly Mock<IAuthenticationHelper> _authenticationHelper;
        private readonly Mock<INavigationService> _navigationService;
        private readonly Mock<IRedditApiConsumer> _redditApiConsumer;
        private readonly Mock<IRestUserRepository> _restUserPreferenceRepository;
        private readonly Mock<IUserHandler> _userHandler;
        private readonly MainPageViewModel _commentableViewModel;

        public MainPageViewModelTest()
        {
            _authenticationHelper = new Mock<IAuthenticationHelper>();
            _navigationService = new Mock<INavigationService>();
            _redditApiConsumer = new Mock<IRedditApiConsumer>();
            _restUserPreferenceRepository = new Mock<IRestUserRepository>();
            _userHandler = new Mock<IUserHandler>();

            _commentableViewModel = new MainPageViewModel(
                _authenticationHelper.Object,
                _navigationService.Object,
                _redditApiConsumer.Object,
                _restUserPreferenceRepository.Object,
                _userHandler.Object
            );
        }

        //[Fact(DisplayName = "Generate Posts test if posts is assigned correctly")]
        //public async void GeneratePostsTestIfPostIsAssignedCorrectly()
        //{
        //    //Arrange
        //    var returnResult = Task.FromResult((HttpStatusCode.OK, new ObservableCollection<Post>()
        //    {
        //        new Post(){title = "TitleA"},
        //        new Post(){title = "TitleB"}
        //    }));
        //    _redditApiConsumer.Setup(o => o.GetHomePageContent())
        //                        .Returns(returnResult);
        //    //_restUserPreferenceRepository.Setup(o => o.CreateAsync(It.IsAny<Entities.GorillaEntities.User>()));
        //    //Act
        //    await _commentableViewModel.GeneratePosts();

        //    //Assert
        //    var expectedCount = 2;
        //    var actualCount = _commentableViewModel.Posts.Count;
        //    Assert.Equal(expectedCount, actualCount);
        //}
    }
}
