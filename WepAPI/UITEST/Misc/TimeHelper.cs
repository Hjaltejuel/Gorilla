using Entities.RedditEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gorilla.Model
{
    public static class TimeHelper
    {
        public static string CalcCreationDate(AbstractCommentable commentable)
        {
            string CreationInfo = "Added ";
            int timeInt = commentable.created_utc;
            DateTime unix = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            DateTime time = unix.AddSeconds(timeInt);

            int DaysSince = (int)(DateTime.Now - time).TotalDays;
            if (DaysSince == 0)
            {
                int secondsSince = (int)(DateTime.Now - time).TotalSeconds;
                if (secondsSince < 10)
                    CreationInfo += "just now";
                else if (secondsSince < 60)
                    CreationInfo += secondsSince + " seconds ago";
                else if (secondsSince < 120)
                    CreationInfo += "a minute ago";
                else if (secondsSince < 3600)
                    CreationInfo += (secondsSince / 60) + " minutes ago";
                else if (secondsSince < 7200)
                    CreationInfo += "an hour ago";
                else if (secondsSince < 86400)
                    CreationInfo += (secondsSince / 3600) + " hours ago";
            }
            else if (DaysSince == 1)
                CreationInfo += "Yesterday";
            else if (DaysSince < 31)
                CreationInfo += DaysSince + " days ago";
            else if (DaysSince < 365)
                CreationInfo += (DaysSince / 30) + " months ago";
            else CreationInfo += (DaysSince / 365) + " years ago";

            return CreationInfo;
        }

        public static string CalcCreationDateByUser(AbstractCommentable commentable)
        {
            var CreationInfo = CalcCreationDate(commentable);
            return (CreationInfo += " by " + commentable.author);
        }


    }
}
