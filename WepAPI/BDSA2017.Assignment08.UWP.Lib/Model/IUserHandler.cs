using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.RedditEntities;
using UI.Lib.Model.RedditRestInterfaces;


namespace UI.Lib.Model
{
    public interface IUserHandler
    {
        Task SetUser(IRedditApiConsumer user);
        User GetUser();
        string GetUserName();
        byte[] GetProfilePic();
    }
}
