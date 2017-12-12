using System.Collections.ObjectModel;

namespace Entities.RedditEntities
{
    public abstract class AbstractCommentable
    {
        public ObservableCollection<Comment> Replies { get; set; }
        public int created_utc { get; set; }
        public string author { get; set; }
        public int score { get; set; }
        public string likes { get; set; }
        public bool can_mod_post { get; set; }
        public string name { get; set; }
        public int downs { get; set; }
        public int ups { get; set; }
        public int created { get; set; }
        public bool archived { get; set; }
        public bool edited { get; set; }
        public bool saved { get; set; }
        public string report_reasons { get; set; }
        public string approved_at_utc { get; set; }
        public string banned_by { get; set; }
        public string subreddit { get; set; }
        public string[] user_reports { get; set; }
        public string id { get; set; }
        public string banned_at_utc { get; set; }
        public string approved_by { get; set; }
        public string author_flair_css_class { get; set; }
        public string author_flair_text { get; set; }
        public int gilded { get; set; }
        public string stickied { get; set; }
        public string subreddit_name_prefixed { get; set; }
        public string subreddit_type { get; set; }
        public string distinguished { get; set; }
        public string[] mod_reports { get; set; }

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