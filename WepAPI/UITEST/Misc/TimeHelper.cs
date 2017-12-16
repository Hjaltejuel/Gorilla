using System;
using Entities.RedditEntities;

namespace UITEST.Misc
{
    public static class TimeHelper
    {
        public static string CalcCreationDate(AbstractCommentable commentable)
        {
            string creationInfo = "Added ";
            int timeInt = commentable.created_utc;
            DateTime unix = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime time = unix.AddSeconds(timeInt);

            int daysSince = (int)(DateTime.Now - time).TotalDays;
            if (daysSince == 0)
            {
                int secondsSince = (int)(DateTime.Now - time).TotalSeconds;
                if (secondsSince < 10)
                    creationInfo += "just now";
                else if (secondsSince < 60)
                    creationInfo += secondsSince + " seconds ago";
                else if (secondsSince < 120)
                    creationInfo += "a minute ago";
                else if (secondsSince < 3600)
                    creationInfo += (secondsSince / 60) + " minutes ago";
                else if (secondsSince < 7200)
                    creationInfo += "an hour ago";
                else if (secondsSince < 86400)
                    creationInfo += (secondsSince / 3600) + " hours ago";
            }
            else if (daysSince == 1)
                creationInfo += "Yesterday";
            else if (daysSince < 31)
                creationInfo += daysSince + " days ago";
            else if (daysSince < 365)
                creationInfo += (daysSince / 30) + " months ago";
            else creationInfo += (daysSince / 365) + " years ago";

            return creationInfo;
        }

        public static string CalcCreationDateByUser(AbstractCommentable commentable)
        {
            var creationInfo = CalcCreationDate(commentable);
            return creationInfo + " by " + commentable.author;
        }


    }
}
