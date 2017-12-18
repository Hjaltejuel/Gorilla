using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UITEST.Model.GorillaRestInterfaces;
using UITEST.Model.GorillaRepositories;
using Xunit;
using Moq;
using Entities.GorillaEntities;
using UITEST.ViewModel;
using Entities.RedditEntities;
using UITEST.Model;
using UITEST.Authentication.GorillaAuthentication;
using UITEST.Model.RedditRestInterfaces;
using XUnit;

namespace BDSA2017.Assignment08.UWP.Tests.GorillaRepoTests
{
    public class CreatePostPageViewModelTests
    {
        private readonly Mock<IRestPostRepository> helper;
        private readonly Mock<INavigationService> service;
        private readonly Mock<IRestUserPreferenceRepository> repository;
        private readonly Mock<IRedditApiConsumer> consumer;
        public CreatePostPageViewModelTests()
        {

            service = new Mock<INavigationService>();
            helper = new Mock<IRestPostRepository>();
            repository = new Mock<IRestUserPreferenceRepository>();
            consumer = new Mock<IRedditApiConsumer>();

            _commentableViewModel = new CommentableViewModel(
                _navigationService.Object,
                _restUserPreferenceRepository.Object,
                _redditApiConsumer.Object
            );
        }
        [Fact]
        public void Does_Post_Disliked_Call_Reddit()
        {
            var mock = new Mock<CommentableViewModel>();
            var vm = new PostPageViewModel(service.Object, helper.Object, repository.Object, consumer.Object);

            vm.PostDislikedAsync();

            mock.Verify(a => a.LikeCommentableAsync(new Comment {}, -1), Times.Once);
        }
    }
}
