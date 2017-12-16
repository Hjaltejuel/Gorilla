using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Entities.GorillaEntities
{
    
    public class User
    {
       
      
        [Key]
        [StringLength(50)]
        [JsonProperty("username")]
        public string Username { get; set; }


        [StringLength(100)]
        [JsonProperty("pathToProfilePicture")]
        public string PathToProfilePicture { get; set; }

   

    }
}
