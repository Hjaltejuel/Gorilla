using System.Threading.Tasks;
using Entities.GorillaEntities;
using UI.Lib.Authentication.GorillaAuthentication;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;

namespace UI.Lib.ViewModel
{
    public class MainPageViewModel : SearchableViewModel
    {
        private readonly IUserHandler _userHandler;
        public delegate void MainReady();
        public event MainReady MainReadyEvent;
        public MainPageViewModel(IAuthenticationHelper helper, INavigationService service, IRedditApiConsumer consumer, IRestUserRepository repository, IUserHandler userHandler) : base( service, consumer)
        {
            Repository = repository;
            _userHandler = userHandler;
            Initialize();
        }

        public async Task GeneratePosts()
        {
         
            Posts = (await Consumer.GetHomePageContent()).Item2;
            MainReadyEvent.Invoke();
        }
        public async Task Initialize()
        {
            await GeneratePosts();
        }
    }
}
