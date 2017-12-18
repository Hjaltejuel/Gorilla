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
        public MainPageViewModel(INavigationService service, IRedditApiConsumer consumer, IRestUserRepository repository) : base( service, consumer)
        {
            Repository = repository;
            Initialize();
        }

        public async Task GeneratePosts()
        {
            InvokeLoadSwitchEvent();
            Posts = (await Consumer.GetHomePageContent()).Item2;
            InvokeLoadSwitchEvent();
        }
        public async Task Initialize()
        {
            await GeneratePosts();
        }
    }
}
