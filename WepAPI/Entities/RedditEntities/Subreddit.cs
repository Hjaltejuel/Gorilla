using System.Collections.ObjectModel;

namespace Entities.RedditEntities
{
    public class Subreddit
    {
        public string interest { get; set; }
        public string user_is_contributor { get; set; }
        public string banner_img { get; set; }
        public string user_flair_text { get; set; }
        public string submit_text_html { get; set; }
        public string user_is_banned { get; set; }
        public bool? wiki_enabled { get; set; }
        public bool show_media { get; set; }
        public string id { get; set; }
        public string description { get; set; }
        public string submit_text { get; set; }
        public string user_can_flair_in_sr { get; set; }
        public string display_name { get; set; }
        public string header_img { get; set; }
        public string description_html { get; set; }
        public string title { get; set; }
        public bool collapse_deleted_comments { get; set; }
        public string user_has_favorited { get; set; }
        public string public_description { get; set; }
        public bool over18 { get; set; }
        public string public_description_html { get; set; }
        public bool spoilers_enabled { get; set; }
        public int[] icon_size { get; set; }
        public string audience_target { get; set; }
        public string suggested_comment_sort { get; set; }
        public int? active_user_count { get; set; }
        public string icon_img { get; set; }
        public string header_title { get; set; }
        public string display_name_prefixed { get; set; }
        public string user_is_muted { get; set; }
        public string submit_link_label { get; set; }
        public int? accounts_active { get; set; }
        public bool public_traffic { get; set; }
        public int[] header_size { get; set; }
        public int subscribers { get; set; }
        public string user_flair_css_class { get; set; }
        public string submit_text_label { get; set; }
        public string key_color { get; set; }
        public string user_sr_flair_enabled { get; set; }
        public string lang { get; set; }
        public string is_enrolled_in_new_modmail { get; set; }
        public string whitelist_status { get; set; }
        public string name { get; set; }
        public bool user_flair_enabled_in_sr { get; set; }
        public int created { get; set; }
        public string url { get; set; }
        public bool quarantine { get; set; }
        public bool hide_ads { get; set; }
        public int created_utc { get; set; }
        public int[] banner_size { get; set; }
        public string user_is_moderator { get; set; }
        public bool allow_discovery { get; set; }
        public bool accounts_active_is_fuzzed { get; set; }
        public string advertiser_category { get; set; }
        public bool user_sr_theme_enabled { get; set; }
        public bool link_flair_enabled { get; set; }
        public bool allow_images { get; set; }
        public bool show_media_preview { get; set; }
        public int comment_score_hide_mins { get; set; }
        public string subreddit_type { get; set; }
        public string submission_type { get; set; }
        public string user_is_subscriber { get; set; }
        public ObservableCollection<Post> posts { get; set; }
    }
}
