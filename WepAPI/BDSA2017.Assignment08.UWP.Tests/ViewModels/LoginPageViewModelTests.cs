using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Lib.Authentication;
using UI.Lib.Authentication.GorillaAuthentication;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;
using UI.Lib.ViewModel;
using Xunit;

namespace UI.Test.ViewModels
{
    public class LoginPageViewModelTests
    {
        private readonly Mock<INavigationService> _navigationService;
        private readonly Mock<IRedditAuthHandler> _authHandler;
        private readonly Mock<IAuthenticationHelper> _authHelper;
        private readonly Mock<IRedditApiConsumer> _redditAPIConsumer;
        private readonly Mock<IRestUserRepository> _repository;
        private readonly Mock<IUserHandler> _userHandler;
        private readonly LoginPageViewModel _loginPageViewModel;
        
        public LoginPageViewModelTests()
        {
            _navigationService = new Mock<INavigationService>();
            _authHandler = new Mock<IRedditAuthHandler>();
            _authHelper = new Mock<IAuthenticationHelper>();
            _redditAPIConsumer = new Mock<IRedditApiConsumer>();
            _repository = new Mock<IRestUserRepository>();
            _userHandler = new Mock<IUserHandler>();

            _loginPageViewModel = new LoginPageViewModel(
                _navigationService.Object,
                _authHandler.Object,
                _authHelper.Object,
                _repository.Object,
                _redditAPIConsumer.Object,
                _userHandler.Object
            );
        }

        [Fact(DisplayName = "Is authHandler called")]
        public void Is_AuthHandler_And_HasAuthenticated_Called()
        {
            //Act
            _loginPageViewModel.BeginAuthentication();

            //Assert
            _authHandler.Verify(v => v.BeginAuth(), Times.Once());
        }

        [Fact(DisplayName = "Is Logout called")]
        public void Is_Logout_Called()
        {
            //Act
            _loginPageViewModel.LogOut();

            //Assert
            _authHandler.Verify(v => v.LogOut(), Times.Once());
        }
    }
}
