using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.GorillaEntities;
using Entities.RedditEntities;
using UITEST.Model;
using UITEST.Model.GorillaRestInterfaces;
using UITEST.Model.RedditRestInterfaces;
using UITEST.View;

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
            var newComment = (await _redditApiConsumer.CreateCommentAsync(abstractCommentableToCommentOn, newCommentBody)).Item2;
            if (newComment == null) return null;

            abstractCommentableToCommentOn.Replies.Insert(0, newComment);
            //No need to await this
            _restUserPreferenceRepository.UpdateAsync(new UserPreference
            {
                Username = UserFactory.GetInfo().name,
                SubredditName = newComment.subreddit,
                PriorityMultiplier = 3
            });
            return newComment;
        }
    }
}
