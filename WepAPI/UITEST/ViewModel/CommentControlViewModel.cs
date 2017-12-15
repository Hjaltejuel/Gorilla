
using Entities.RedditEntities;
using Gorilla.Model;
using Gorilla.Model.GorillaRestInterfaces;
using Model;
using UITEST.RedditInterfaces;
using Windows.UI.Xaml;

namespace UITEST.ViewModel
{
    class CommentControlViewModel : CommentableViewModel
    {
        public delegate void Comments();
        public event Comments CommentsReadyEvent;
        IRedditAPIConsumer _redditAPIConsumer;
        IRestUserPreferenceRepository _restUserPreferenceRepository;
        IRestPostRepository _repository;
        private Comment _currentPost;
        public Comment CurrentPost { get { return _currentPost; } set { _currentPost = value; OnPropertyChanged(); } }

        public CommentControlViewModel(INavigationService service, IRestPostRepository repository, IRestUserPreferenceRepository restUserPreferenceRepository, IRedditAPIConsumer redditAPIConsumer) :  base(service, repository, restUserPreferenceRepository, redditAPIConsumer)
        {
            _repository = repository;
            _restUserPreferenceRepository = restUserPreferenceRepository;
            _redditAPIConsumer = redditAPIConsumer;
        }
        public async void GetCurrentPost(Comment post)
        {
            /*
             * CurrentPost = await _redditAPIConsumer.GetPostAndCommentsByIdAsync(post.id);
            await _repository.CreateAsync(new Entities.Post { Id = post.id, username = UserFactory.GetInfo().name });
            CommentsReadyEvent.Invoke();
            */
        }

        public void Initialize(Comment comment)
        {
            base.Initialize(comment);
            _currentPost = comment;
            //_repository.CreateAsync(new Entities.Post { Id = post.id, username = UserFactory.GetInfo().name });
            //CurrentPost = post;
            //votes = CurrentPost.score;
            //GetCurrentPost(post);
            //timeSinceCreation = TimeHelper.CalcCreationDateByUser(CurrentPost);
            likeButton = App.Current.Resources["LikeButton"] as Style;
            dislikeButton = App.Current.Resources["DislikeButton"] as Style;
        }
    }
}
