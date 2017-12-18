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
        private readonly Mock<IRedditAuthHandler> _handler;
        private readonly LoginPageViewModel _lpvm;
        
        public LoginPageViewModelTests()
        {

            _navigationService = new Mock<INavigationService>();
            _handler = new Mock<IRedditAuthHandler>();
            _lpvm = new LoginPageViewModel(
                _navigationService.Object,
                _handler.Object
            );
        }

        [Fact(DisplayName = "Is authHandler called")]
        public void Is_AuthHandler_And_HasAuthenticated_Called()
        {
            //Act
            _lpvm.BeginAuthentication();

            //Assert
            _handler.Verify(v => v.BeginAuth(), Times.Once());
        }

        [Fact(DisplayName = "Is Logout called")]
        public void Is_Logout_Called()
        {
            //Act
            _lpvm.LogOut();

            //Assert
            _handler.Verify(v => v.LogOut(), Times.Once());
        }
    }
}
