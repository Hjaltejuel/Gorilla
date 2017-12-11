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
        public string Username { get; set; }


        [StringLength(100)]
        public string PathToProfilePicture { get; set; }

   

    }
}
