using UITEST.Authentication;
using UITEST.Model;
using UITEST.Model.RedditRepositories;
using UITEST.View;

namespace UITEST.ViewModel
{
    class LoginPageViewModel : BaseViewModel
    {
        public LoginPageViewModel(INavigationService service) : base(service)
        {
            BeginAuthentication();
        }

        public async void BeginAuthentication()
        {
            var authHandler = new RedditAuthHandler();
            await authHandler.BeginAuth();
            HasAuthenticated();
        }
        public void HasAuthenticated()
        {
            Service.Navigate(typeof(MainPage), null);
        }
    }
}
