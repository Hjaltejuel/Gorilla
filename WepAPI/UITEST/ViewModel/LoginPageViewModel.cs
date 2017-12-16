using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gorilla.Model;
using UITEST.ViewModel;
using UITEST.Authentication;
using Windows.UI.Xaml.Controls;
using UITEST.View;

namespace Gorilla.ViewModel
{
    class LoginPageViewModel : BaseViewModel
    {
        public LoginPageViewModel(INavigationService service) : base(service)
        {
            BeginAuthentication();
        }

        public async void BeginAuthentication()
        {
            var AuthHandler = new RedditAuthHandler();
            await AuthHandler.BeginAuth();
            HasAuthenticated();
        }
        public void HasAuthenticated()
        {
            _service.Navigate(typeof(MainPage), null);
        }
    }
}
