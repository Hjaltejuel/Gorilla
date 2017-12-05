using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Comment
    {
        public string Api_type{ get; set; }
        public JToken Richtext_json { get; set; }
        public string Thing_id { get; set; }
        public string Text { get; set; }
        public string Modhash { get; set; }
    }
}
