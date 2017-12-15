using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.RedditEntities
{
    public class User
    {
        public byte[] ProfilePic { get; set; }
        public bool is_employee { get; set; }
        public bool has_visited_new_profile { get; set; }
        public bool pref_no_profanity { get; set; }
        public bool is_suspended { get; set; }
        public string pref_geopopular { get; set; }
        public JToken subreddit { get; set; }
        public bool is_sponsor { get; set; }
        public string gold_expiration { get; set; }
        public string id { get; set; }
        public string suspension_expiration_utc { get; set; }
        public bool verified { get; set; }
        public string new_modmail_exists { get; set; }
        public JToken features { get; set; }
        public bool over_18 { get; set; }
        public bool is_gold { get; set; }
        public bool is_mod { get; set; }
        public bool has_verified_email { get; set; }
        public bool  in_redesign_beta { get; set; }
        public string icon_img { get; set; }
        public bool has_mod_mail { get; set; }
        public string oauth_client_id { get; set; }
        public bool hide_from_robots { get; set; }
        public int link_karma { get; set; }
        public int inbox_count { get; set; }
        public bool pref_top_karma_subreddits { get; set; }
        public bool has_mail { get; set; }
        public bool pref_show_snoovatar { get; set; }
        public string name { get; set; }
        public int created { get; set; }
        public int gold_creddits { get; set; }
        public int created_utc { get; set; }
        public bool in_beta { get; set; }
        public int comment_karma { get; set; }
        public bool has_subscribed { get; set; }
    }

}
