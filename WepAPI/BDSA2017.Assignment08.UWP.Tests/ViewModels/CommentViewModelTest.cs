using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using Entities.RedditEntities;
using Moq;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;
using UI.Lib.ViewModel;
using Xunit;

namespace UI.Test.ViewModels
{
    public class CommentViewModelTest
    {
        private readonly Mock<INavigationService> _navigationService;
        private readonly Mock<IRestUserPreferenceRepository> _restUserPreferenceRepository;
        private readonly Mock<IRedditApiConsumer> _redditApiConsumer;
        private readonly CommentViewModel _commentViewModel;

        public CommentViewModelTest()
        {
            _navigationService = new Mock<INavigationService>();
            _restUserPreferenceRepository = new Mock<IRestUserPreferenceRepository>();
            _redditApiConsumer = new Mock<IRedditApiConsumer>();

            _commentViewModel = new CommentViewModel(
                _navigationService.Object,
                _restUserPreferenceRepository.Object,
                _redditApiConsumer.Object
            );
        }

        [Fact(DisplayName = "Get more comments succesful")]
        public async void GetChildComments_Get_more_comments_successful()
        {
            //Arrange
            var parentComment = new Comment() {name = "parent_id"};
            var childComment = new Comment() { parent_id = "parent_id", name = "child_id" };

            var returnResult = Task.FromResult(
                (HttpStatusCode.OK, new ObservableCollection<Comment>()
                {
                    parentComment, //parent comment
                    childComment //child comment
                })
            );

            _redditApiConsumer.Setup(o => o.GetMoreComments(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(returnResult);


            //Act
            var actual =
                await _commentViewModel.GetChildComments(It.IsAny<string>(), It.IsAny<string[]>(), new Comment());
            
            //Assert
            Assert.Equal(parentComment.Replies[0].name, childComment.name);
        }

        [Fact(DisplayName = "Get more comments unsuccesful")]
        public async void GetChildComments_Get_more_comments_unsuccessful()
        {
            //Arrange
            var parentObject = new Comment()
            {
                depth = 1
            };

            var returnResult = Task.FromResult((HttpStatusCode.BadRequest, (ObservableCollection<Comment>)null));

            _redditApiConsumer.Setup(o => o.GetMoreComments(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(returnResult);

            Comment expected = null;

            //Act
            var actual = await _commentViewModel.CreateComment(parentObject, "CommentBody");

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
