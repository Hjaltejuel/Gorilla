using UI.Lib.Authentication;

using UI.Lib.Authentication;
using UI.Lib.Model;
using UI.Lib.Model.RedditRepositories;


namespace UI.Lib.ViewModel
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
