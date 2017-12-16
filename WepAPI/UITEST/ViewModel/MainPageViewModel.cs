using Entities.RedditEntities;
using Gorilla.AuthenticationGorillaAPI;
using Gorilla.Model;
using Gorilla.ViewModel;
using Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UITEST.RedditInterfaces;
using UITEST.View;

namespace UITEST.ViewModel
{
    public class MainPageViewModel : SearchableViewModel
    {
        public MainPageViewModel(IAuthenticationHelper helper, INavigationService service, IRedditAPIConsumer consumer, IRestUserRepository repository) : base(helper, service, consumer)
        {
            _repository = repository;
            Initialize();
        }

        public async Task GeneratePosts()
        {
            InvokeLoadSwitchEvent();
            await UserFactory.initialize(_consumer);
            await _repository.CreateAsync(new Entities.User { Username = UserFactory.GetInfo().name, PathToProfilePicture = "profilePicture.jpg" });
            Posts = await _consumer.GetHomePageContent();
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
                if(firstTime == true)
                {
                    firstTime = false;
                }
                else
                {
                    await Initialize();
                }
            }
        }
    }
}
