using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Entities.GorillaEntities;
using Entities.RedditEntities;
using UITEST.Model;
using UITEST.Model.GorillaRestInterfaces;
using UITEST.Model.RedditRestInterfaces;
using Windows.UI.Core;

namespace UITEST.ViewModel
{
    public class CommentableViewModel : BaseViewModel
    {
        private readonly IRedditApiConsumer _redditApiConsumer;
        private readonly IRestUserPreferenceRepository _restUserPreferenceRepository;
        public CommentableViewModel(INavigationService service, IRestPostRepository repository, IRestUserPreferenceRepository restUserPreferenceRepository, IRedditApiConsumer redditApiConsumer) : base(service)
        {
            _redditApiConsumer = redditApiConsumer;
            _restUserPreferenceRepository = restUserPreferenceRepository;
        }
        public async Task<Comment> CreateComment(AbstractCommentable abstractCommentableToCommentOn, string newCommentBody)
        {
            var commentResponse = await _redditApiConsumer.CreateCommentAsync(abstractCommentableToCommentOn, newCommentBody);
            if (commentResponse.Item1 != HttpStatusCode.OK) return null;

            var newComment = commentResponse.Item2;
            _restUserPreferenceRepository.UpdateAsync(new UserPreference
            {
                Username = newComment.author,
                SubredditName = newComment.subreddit,
                PriorityMultiplier = 3
            });
            return newComment;
        }
        public async Task LikeCommentableAsync(AbstractCommentable commentable, int direction)
        {
            await _redditApiConsumer.VoteAsync(commentable, direction);
            if (direction == 0) return;
            await _restUserPreferenceRepository.UpdateAsync(new UserPreference { Username = UserFactory.GetInfo().name, SubredditName = commentable.subreddit, PriorityMultiplier = 1 });
        }
        public void SetHandCursor()
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
        }
        public void SetArrowCursor()
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        }
    }
}
