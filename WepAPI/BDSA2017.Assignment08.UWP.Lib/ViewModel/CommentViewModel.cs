using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Entities.RedditEntities;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;

namespace UI.Lib.ViewModel
{
    public class CommentViewModel : CommentableViewModel
    {
        readonly IRedditApiConsumer _redditApiConsumer;
        public CommentViewModel(INavigationService service, IRestUserPreferenceRepository restUserPreferenceRepository, IRedditApiConsumer redditApiConsumer) 
            : base(service,restUserPreferenceRepository,redditApiConsumer)
        {
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
                if (comment.parent_id == null) { continue;}
                if (dict.TryGetValue(comment.parent_id, out var c))
                {
                    c.Replies.Add(comment);
                }
            }
            return finalList;
        }
    }
}
