using BDSA2017.Assignment08.UWP.Authentication;

using UITEST.Authentication;
using UITEST.Model;
using UITEST.Model.RedditRepositories;


namespace UITEST.ViewModel
{
    public class LoginPageViewModel : BaseViewModel
    {
        private readonly IRedditAuthHandler _authHandler;

        public LoginPageViewModel(INavigationService service, IRedditAuthHandler authHandler) : base(service)
        {
            _authHandler = authHandler;
        }

        public async void BeginAuthentication()
        {
            await _authHandler.BeginAuth();
            HasAuthenticated();
        }

        public void HasAuthenticated()
        {
            Service.Navigate(MainPage, null);
        }

        public void LogOut()
        {
            _authHandler.LogOut();
        }
    }
}
