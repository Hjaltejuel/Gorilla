using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Entities.RedditEntities;
using UITEST.Model;
using UITEST.Model.GorillaRestInterfaces;
using UITEST.Model.RedditRestInterfaces;

namespace UITEST.ViewModel
{
    public class CommentViewModel : CommentableViewModel
    {
        IRedditApiConsumer _redditApiConsumer;
        IRestUserPreferenceRepository _restUserPreferenceRepository;
        IRestPostRepository _repository;
        public CommentViewModel(INavigationService service, IRestPostRepository repository, IRestUserPreferenceRepository restUserPreferenceRepository, IRedditApiConsumer redditApiConsumer) 
            : base(service,repository,restUserPreferenceRepository,redditApiConsumer)
        {
            _repository = repository;
            _restUserPreferenceRepository = restUserPreferenceRepository;
            _redditApiConsumer = redditApiConsumer;
        }

        public async Task<ObservableCollection<Comment>> GetChildComments(string postId, string[] children, Comment _currentComment)
        {
            var list = (await _redditApiConsumer.GetMoreComments(postId, children, _currentComment.depth)).Item2;
            var dict = new Dictionary<string, Comment>();
            var finalList = new ObservableCollection<Comment>();
            foreach (var comment in list)
            {
                dict.Add(comment.name, comment);
                if (comment.depth == _currentComment.depth) finalList.Add(comment);
            }
            foreach (var comment in list)
            {
                if (dict.TryGetValue(comment.parent_id, out var c))
                {
                    c.Replies.Add(comment);
                }
            }
            return finalList;
        }

    }
}
