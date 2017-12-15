using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace Entities.RedditEntities
{
    // TODO: FIX STRING LENGTHS
    public class Post : AbstractCommentable
    {
        public string domain { get; set; }
        public JToken media_embed { get; set; }
        public string thumbnail_width { get; set; }
        public string selftext_html { get; set; }
        public string selftext { get; set; }
        public string suggested_sort { get; set; }
        public JToken secure_media { get; set; }
        public bool is_reddit_media_domain { get; set; }
        public string link_flair_text { get; set; }
        public string id { get; set; }
        public string banned_at_utc { get; set; }
        public string view_count { get; set; }
        public bool clicked { get; set; }
        public string title { get; set; }
        public int num_crossposts { get; set; }
        public bool is_crosspostable { get; set; }
        public bool pinned { get; set; }
        public string approved_by { get; set; }
        public bool over_18 { get; set; }
        public bool hidden { get; set; }
        public int num_comments { get; set; }
        private string _thumbnail;
        public string thumbnail
        {
            get
            {
                if (is_self)
                    return "/Assets/Textpost.png";
                else if(_thumbnail=="default")
                    return "/Assets/Externallink.png";
                return _thumbnail;
            }
            set { _thumbnail = value; }
        }
        public string subreddit_id { get; set; }
        public bool hide_score { get; set; }
        public string link_flair_css_class { get; set; }
        public string author_flair_css_class { get; set; }
        public bool contest_mode { get; set; }
        public int gilded { get; set; }
        public bool locked { get; set; }
        public bool brand_safe { get; set; }
        public JToken secure_media_embed { get; set; }
        public string removal_reason { get; set; }
        public bool can_gild { get; set; }
        public string thumbnail_height { get; set; }
        public string parent_whitelist_status { get; set; }
        public bool spoiler { get; set; }
        public string permalink { get; set; }
        public string num_reports { get; set; }
        public string whitelist_status { get; set; }
        public string stickied { get; set; }
        public string url { get; set; }
        public string author_flair_text { get; set; }
        public bool quarantine { get; set; }
        public string subreddit_name_prefixed { get; set; }
        public string distinguished { get; set; }
        public JToken media { get; set; }
        public double upvote_ratio { get; set; }
        public string[] mod_reports { get; set; }
        public bool is_self { get; set; }
        public bool visited { get; set; }
        public string subreddit_type { get; set; }
        public bool is_video { get; set; }

        public Post()
        {
            Replies = new ObservableCollection<Comment>();
        }
        // er det saadan det skal virke? 
        public string PostSerialize() {
            
            string output = JsonConvert.SerializeObject(this);
            return output;
        }
    }
}
