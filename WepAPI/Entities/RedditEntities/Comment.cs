using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Entities.RedditEntities
{
    public class Comment : AbstractCommentable
    {
        public string subreddit_id { get; set; }
        public string removal_reason { get; set; }
        public string link_id { get; set; }
        public Listing replies
        {
            get => null;
            set => BuildReplies(value);
        }
        public int count { get; set; }
        public string[] children { get; set; }
        public string parent_id { get; set; }
        public string body { get; set; }
        public bool collapsed { get; set; }
        public bool is_submitter { get; set; }
        public string collapsed_reason { get; set; }
        public string body_html { get; set; }
        public bool can_gild { get; set; }
        public bool score_hidden { get; set; }
        public string permalink { get; set; }
        public int controversiality { get; set; }
        public int depth { get; set; }
        public string num_reports { get; set; }

        public Comment()
        {
            Replies = new ObservableCollection<Comment>();
        }
        
    }
}
