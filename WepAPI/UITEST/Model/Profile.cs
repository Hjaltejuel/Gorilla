using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITEST.Model
{
    public class Profile
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int KarmaGiven { get; set; }
        public int KarmaRecieved { get; set; }
        public int PostCreated { get; set; }
        public int AmountOfSubRedditsSubscribedTo { get; set; } 

        private DateTime joinDate;  
        public DateTime JoinDate
        {
            get { return joinDate.Date; }
            set { joinDate = value; }
        }

        public string PathToProfilePicture { get; set; }
    }
}
