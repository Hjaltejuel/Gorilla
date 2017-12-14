using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
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
