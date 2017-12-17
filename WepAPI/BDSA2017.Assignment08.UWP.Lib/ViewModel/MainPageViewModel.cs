using System.Threading.Tasks;
using Entities.GorillaEntities;
using UITEST.Authentication.GorillaAuthentication;
using UITEST.Model;
using UITEST.Model.GorillaRestInterfaces;
using UITEST.Model.RedditRestInterfaces;

namespace UITEST.ViewModel
{
    public class MainPageViewModel : SearchableViewModel
    {
        public MainPageViewModel(IAuthenticationHelper helper, INavigationService service, IRedditApiConsumer consumer, IRestUserRepository repository) : base(helper, service, consumer)
        {
            Repository = repository;
            Initialize();
        }

        public async Task GeneratePosts()
        {
            InvokeLoadSwitchEvent();
            await UserFactory.Initialize(Consumer);
            await Repository.CreateAsync(new User { Username = UserFactory.GetInfo().name, PathToProfilePicture = "profilePicture.jpg" });
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
