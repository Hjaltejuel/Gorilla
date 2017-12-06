using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace WebApplication2.Models
{
    // TODO: FIX STRING LENGTHS

    public class Post : ICommentable
    {

        public bool ad { get; set; }
        public string api_type = "json";
        public string flair_id { get; set; } // only 36 chars
        public string flair_text { get; set; } // only 64 chars
        public string kind { get; set; }
        public bool nsfw { get; set; }
        public bool resubmit { get; set; }
        public bool sendReplies { get; set; }
        public bool spoiler { get; set; }
        public string subredditName { get; set; }
        public string title { get; set; } // only 300 chars 
        public string url { get; set; }
        public ObservableCollection<Comment> comments { get; set; }

        // er det saadan det skal virke? 
        public string postSerialize() {
            
            string output = JsonConvert.SerializeObject(this);
            return output;
        }   

        public Post postDeSerialize(String serializedPost)
        {
            Post output = JsonConvert.DeserializeObject<Post>(serializedPost);
            return output;
        }
    }

   
}
