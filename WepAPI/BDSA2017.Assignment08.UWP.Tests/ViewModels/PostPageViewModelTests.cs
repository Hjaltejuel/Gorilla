using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;
using UI.Lib.ViewModel;
using xunit;

namespace UI.Test.ViewModels
{
    public class PostPageViewModelTests
    {
        private readonly Mock<INavigationService> _navigationService;
        private readonly Mock<IRestPostRepository> _restPostRepository;
        private readonly Mock<IRestUserPreferenceRepository> _restUserPreferenceRepository;
        private readonly Mock<IRedditApiConsumer> _redditApiConsumer;
        private readonly Mock<IUserHandler> _userHandler;
        private readonly PostPageViewModel _postPageViewModel;

        public PostPageViewModelTests()
        {
            _navigationService = new Mock<INavigationService>();
            _restPostRepository = new Mock<IRestPostRepository>();
            _restUserPreferenceRepository = new Mock<IRestUserPreferenceRepository>();
            _redditApiConsumer = new Mock<IRedditApiConsumer>();
            _userHandler = new Mock<IUserHandler>();

            _postPageViewModel = new PostPageViewModel(
                _navigationService.Object,
                _restPostRepository.Object,
                _restUserPreferenceRepository.Object,
                _redditApiConsumer.Object,
                _userHandler.Object
            );
        }
    }
}
