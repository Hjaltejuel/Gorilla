using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;

namespace WebApplication2.Models
{
    // TODO: FIX STRING LENGTHS

    public class Post : Commentable
    {

        public bool Ad { get; set; }
        public string Api_type = "json";
        public string Flair_id { get; set; } // only 36 chars
        public string Flair_text { get; set; } // only 64 chars
        public string Kind { get; set; }
        public bool Nsfw { get; set; }
        public bool Resubmit { get; set; }
        public bool SendReplies { get; set; }
        public bool Spoiler { get; set; }
        public string SubredditName { get; set; }
        public string Title { get; set; } // only 300 chars 
        public string Url { get; set; }


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
