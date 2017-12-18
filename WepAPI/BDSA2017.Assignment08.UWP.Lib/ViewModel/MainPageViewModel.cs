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
        public MainPageViewModel(IAuthenticationHelper helper, INavigationService service, IRedditApiConsumer consumer, IRestUserRepository repository, IUserHandler userHandler) : base(helper, service, consumer)
        {
            Repository = repository;
            _userHandler = userHandler;
            Initialize();
        }

        public async Task GeneratePosts()
        {
            InvokeLoadSwitchEvent();
            await Repository.CreateAsync(new User { Username = _userHandler.GetUser().name, PathToProfilePicture = "profilePicture.jpg" });
            Posts = (await Consumer.GetHomePageContent()).Item2;
            InvokeLoadSwitchEvent();
        }
        public async Task Initialize()
        {
            if (await Authorize() != null)
            {
                await GeneratePosts();
            }
            else
            {
                if(FirstTime)
                {
                    FirstTime = false;
                }
                else
                {
                    await Initialize();
                }
            }
        }
    }
}
