﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Entities.RedditEntities
{
    public class Comment : AbstractCommentable
    {
        public string subreddit_id { get; set; }
        public string approved_at_utc { get; set; }
        public string banned_by { get; set; }
        public string removal_reason { get; set; }
        public string link_id { get; set; }
        public string likes { get; set; }
        public Listing replies
        {
            get => null;
            set => BuildReplies(value);
        }
        public int count { get; set; }
        public string[] children { get; set; }
        public string[] user_reports { get; set; }
        public bool saved { get; set; }
        public string id { get; set; }
        public string banned_at_utc { get; set; }
        public int gilded { get; set; }
        public bool archived { get; set; }
        public string report_reasons { get; set; }
        public string author { get; set; }
        public bool can_mod_post { get; set; }
        public int ups { get; set; }
        public string parent_id { get; set; }
        public int score { get; set; }
        public string approved_by { get; set; }
        public int downs { get; set; }
        public string body { get; set; }
        public bool edited { get; set; }
        public string author_flair_css_class { get; set; }
        public bool collapsed { get; set; }
        public bool is_submitter { get; set; }
        public string collapsed_reason { get; set; }
        public string body_html { get; set; }
        public bool stickied { get; set; }
        public bool can_gild { get; set; }
        public string subreddit { get; set; }
        public bool score_hidden { get; set; }
        public string permalink { get; set; }
        public string subreddit_type { get; set; }
        public string name { get; set; }
        public int created { get; set; }
        public string author_flair_text { get; set; }
        public int created_utc { get; set; }
        public string subreddit_name_prefixed { get; set; }
        public int controversiality { get; set; }
        public int depth { get; set; }
        public string[] mod_reports { get; set; }
        public string num_reports { get; set; }
        public string distinguished { get; set; }

        public Comment()
        {
            Replies = new ObservableCollection<Comment>();
        }
        
    }
}