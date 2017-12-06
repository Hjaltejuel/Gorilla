using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    class RedditJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Comment));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject json = JObject.Load(reader);
            string value;
            value = (string)json["type"];
            Comment c = new Comment();
            if (value == "t1")
            {
                c.comments = json["data"].ToObject<List<Comment>>(serializer);
            }
            /**else if (value == "t3")
            {
                Post p = new Post();
                p = json["data"].ToObject<List<Dictionary<string, Node>>>(serializer);
            }
            */
            else
            {
                
            }
            return c;
            
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
