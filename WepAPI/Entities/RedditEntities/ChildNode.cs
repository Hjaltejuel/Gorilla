using Newtonsoft.Json.Linq;

namespace Entities.RedditEntities
{
    public class ChildNode
    {
        public string kind { get; set; }
        public JToken data { get; set; }
    }
}