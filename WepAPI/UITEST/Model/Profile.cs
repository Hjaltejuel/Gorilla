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
        public int LinkKarma { get; set; }
        public int CommentKarma { get; set; }
        public string PostCreated { get; set; }
        public string CommentsCreated { get; set; }
        public int AmountOfSubRedditsSubscribedTo { get; set; } 

        private DateTime joinDate;
        public DateTime JoinDate
        {
            get { return joinDate.Date; }
            set { joinDate = value; }
        }
    }
}
