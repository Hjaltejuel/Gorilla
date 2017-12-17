using System;
using System.Net;
using System.Threading.Tasks;
using Entities.RedditEntities;
using UI.Lib.Model.RedditRestInterfaces;

namespace UI.Lib.Model
{
    public class UserHandler : IUserHandler
    {

        private User _user;

        public async Task SetUser(IRedditApiConsumer consumer)
        {
            var accountDetailsResponse = await consumer.GetAccountDetailsAsync();
            if (accountDetailsResponse.Item1 == HttpStatusCode.OK)
            {
                _user = accountDetailsResponse.Item2;
            }
            else
            {
                throw new Exception("Could not log in!");
            }
        }
        public User GetUser()
        {
            return _user;
        }
        public string GetUserName()
        {
            return _user?.name;
        }
        public byte[] GetProfilePic()
        {
            return _user?.ProfilePic;
        }
    }
}
