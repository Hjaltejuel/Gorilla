using Entities.RedditEntities;
using Gorilla.Model;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;
using Windows.UI.Xaml;
using Gorilla.Model.GorillaRestInterfaces;
using UITEST.RedditInterfaces;
using Model;

namespace UITEST.ViewModel
{
    public class PostPageViewModel : CommentableViewModel
    {
        public delegate void Comments();
        public event Comments CommentsReadyEvent;
        IRedditAPIConsumer _redditAPIConsumer;
        IRestUserPreferenceRepository _restUserPreferenceRepository;
        IRestPostRepository _repository;
        private Post _currentPost;
        public Post CurrentPost { get { return _currentPost; } set { _currentPost = value; OnPropertyChanged();  } }
        
        public PostPageViewModel(INavigationService service, IRestPostRepository repository, IRestUserPreferenceRepository restUserPreferenceRepository, IRedditAPIConsumer redditAPIConsumer) :  base(service, repository, restUserPreferenceRepository, redditAPIConsumer)
        {
            _repository = repository;
            _restUserPreferenceRepository = restUserPreferenceRepository;
            _redditAPIConsumer = redditAPIConsumer;
        }
        public async void GetCurrentPost(Post post)
        {
            CurrentPost = await _redditAPIConsumer.GetPostAndCommentsByIdAsync(post.id);
            await _repository.CreateAsync(new Entities.Post { Id = post.id, username = UserFactory.GetInfo().name });
            CommentsReadyEvent.Invoke();
        }
        public void Initialize(Post post)
        {
            base.Initialize(post);
            _repository.CreateAsync(new Entities.Post { Id = post.id, username = UserFactory.GetInfo().name });
            CurrentPost = post;
            GetCurrentPost(post);
        }
    }
}