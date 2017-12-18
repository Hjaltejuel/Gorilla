using System.Net;
using System.Threading.Tasks;
using Entities.GorillaEntities;
using Entities.RedditEntities;
using Moq;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;
using UI.Lib.ViewModel;
using Xunit;

namespace UI.Test.ViewModels
{
    public class CommentableViewModelTest
    {
        private readonly Mock<INavigationService> _navigationService;
        private readonly Mock<IRestPostRepository> _restPostRepository;
        private readonly Mock<IRestUserPreferenceRepository> _restUserPreferenceRepository;
        private readonly Mock<IRedditApiConsumer> _redditApiConsumer;
        private readonly Mock<IUserHandler> _userHandler;
        private readonly CommentableViewModel _commentableViewModel;

        public CommentableViewModelTest()
        {
            _navigationService = new Mock<INavigationService>();
            _restPostRepository = new Mock<IRestPostRepository>();
            _restUserPreferenceRepository = new Mock<IRestUserPreferenceRepository>();
            _redditApiConsumer = new Mock<IRedditApiConsumer>();
            _userHandler = new Mock<IUserHandler>();

            _commentableViewModel = new CommentableViewModel(
                _navigationService.Object,
                _restUserPreferenceRepository.Object,
                _redditApiConsumer.Object,
                _userHandler.Object
            );
        }

        [Fact(DisplayName = "Comment on comment succesful")]
        public async void Comment_on_comment_successful()
        {
            //Arrange
            var parentObject = new Comment();
            var returnResult = Task.FromResult((HttpStatusCode.OK, new Comment { body = "CommentBody" }));
            _redditApiConsumer.Setup(o => o.CreateCommentAsync(parentObject, "CommentBody")).Returns(returnResult);
            var expected = new Comment { body = "CommentBody" };

            //Act
            var actual = await _commentableViewModel.CreateComment(parentObject, "CommentBody");

            //Assert
            Assert.Equal(expected.body, actual.body);
        }

        [Fact(DisplayName = "Create comment unsuccesful")]
        public async void Create_Comment_Unsuccessful()
        {
            //Arrange
            var parentObject = new Comment()
            {
                depth = 1
            };

            var returnResult = Task.FromResult((HttpStatusCode.BadRequest, (Comment)null));

            _redditApiConsumer.Setup(o => o.CreateCommentAsync(parentObject, "CommentBody"))
                .Returns(returnResult);

            Comment expected = null;

            //Act
            var actual = await _commentableViewModel.CreateComment(parentObject, "CommentBody");

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Like comment test")]
        public async void Like_Comment_Test()
        {
            //Arrange
            var parentObject = new Comment() { subreddit = "A" };
            var likeDirection = 1;

            _restUserPreferenceRepository.Setup(v => v.UpdateAsync(new UserPreference())).Returns(Task.FromResult(true));

            //Act
            await _commentableViewModel.LikeCommentableAsync(parentObject, likeDirection);

            //Assert
            _redditApiConsumer.Verify(v => v.VoteAsync(parentObject, likeDirection), Times.Once);
            _restUserPreferenceRepository.Verify(v => v.UpdateAsync(It.IsAny<UserPreference>()), Times.Once);
        }

        [Fact(DisplayName = "Unlike comment test")]
        public async void Unlike_Comment_Test()
        {
            //Arrange
            var parentObject = new Comment();
            var likeDirection = 0;

            //Act
            await _commentableViewModel.LikeCommentableAsync(parentObject, likeDirection);

            //Assert
            _redditApiConsumer.Verify(v => v.VoteAsync(parentObject, likeDirection), Times.Once);
            _restUserPreferenceRepository.Verify(v => v.UpdateAsync(It.IsAny<UserPreference>()), Times.Never);
        }
    }
}
