using UITEST.Authentication;
using UITEST.Model;
using UITEST.Model.RedditRepositories;
using UITEST.View;

namespace UITEST.ViewModel
{
    class LoginPageViewModel : BaseViewModel
    {
        private readonly RedditAuthHandler authHandler;

        public LoginPageViewModel(INavigationService service) : base(service)
        {
            authHandler = new RedditAuthHandler();
        }

        public async void BeginAuthentication()
        {
            await authHandler.BeginAuth();
            HasAuthenticated();
        }

        public void HasAuthenticated()
        {
            Service.Navigate(typeof(MainPage), null);
        }

        public void LogOut()
        {
            authHandler.LogOut();
        }
    }
}
