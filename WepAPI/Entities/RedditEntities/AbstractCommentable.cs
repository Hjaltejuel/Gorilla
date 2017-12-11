using System.Collections.ObjectModel;

namespace Entities.RedditEntities
{
    public abstract class AbstractCommentable
    {
        public ObservableCollection<Comment> Replies { get; set; }

        public void BuildReplies(Listing listing)
        {
            if (listing != null)
            {
                foreach (ChildNode ch in listing.data.children)
                {
                    Replies.Add(ch.data.ToObject<Comment>());
                }
            }
        }
    }
}