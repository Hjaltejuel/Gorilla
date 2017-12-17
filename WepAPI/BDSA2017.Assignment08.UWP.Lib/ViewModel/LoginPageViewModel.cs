using Entities.GorillaEntities;
using System.Threading.Tasks;
using UI.Lib.Authentication;

using UI.Lib.Authentication;
using UI.Lib.Authentication.GorillaAuthentication;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRepositories;
using UI.Lib.Model.RedditRestInterfaces;

namespace UI.Lib.ViewModel
{
    public class LoginPageViewModel : BaseViewModel
    {
        private readonly IRedditAuthHandler _authHandler;
        private readonly IRedditApiConsumer _redditAPIConsumer;
        private readonly IRestUserRepository _repository;

        public LoginPageViewModel(INavigationService service, IRedditAuthHandler authHandler, IAuthenticationHelper helper, IRestUserRepository repository, IRedditApiConsumer redditAPIConsumer) : base(service)
        {
            _redditAPIConsumer = redditAPIConsumer;
            _repository = repository;
            _authHandler = authHandler;
            _gorillaAuthHelper = helper;
            BeginAuthentication();
        }

        public async Task StartupQuestionsAsync()
        {
            await UserFactory.Initialize(_redditAPIConsumer);
            await _repository.CreateAsync(new User { Username = UserFactory.GetInfo().name, PathToProfilePicture = "profilePicture.jpg" });
            if ((await _repository.FindAsync(UserFactory.GetInfo().name)).StartUpQuestionAnswered==0)
            {
                Service.Navigate(StartupQuestionsPage, null);

            }
            else
            {
                HasAuthenticated();
            }
        }
    

        public async void BeginAuthentication()
        {
            await _authHandler.BeginAuth();
            await Authorize();
            await StartupQuestionsAsync();
            
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
