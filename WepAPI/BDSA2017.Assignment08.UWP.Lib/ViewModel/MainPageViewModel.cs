using System.Threading.Tasks;
using Entities.GorillaEntities;
using UI.Lib.Authentication.GorillaAuthentication;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;
using System.Collections.Generic;

namespace UI.Lib.ViewModel
{
    public class MainPageViewModel : SearchableViewModel
    {
        private readonly IUserHandler _userHandler;
        private readonly IRestSubredditRepository _restSubredditRepository;
        public delegate void MainReady();
        public event MainReady MainReadyEvent;
        public MainPageViewModel(IAuthenticationHelper helper, INavigationService service, IRedditApiConsumer consumer, IRestUserRepository repository, IUserHandler userHandler, IRestSubredditRepository restSubredditRepository) : base( service, consumer)
        {
            _restSubredditRepository = restSubredditRepository;
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

        public async Task<IReadOnlyCollection<string>> GetFiltered(string like)
        {
            return await _restSubredditRepository.GetLikeAsync(like);
                
        }
    }
}
