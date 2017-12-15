using Entities.RedditEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gorilla.Model
{
    public static class UserFactory
    {

        private static User user;

        public async static Task initialize(IRedditAPIConsumer consumer)
        {
            user = await consumer.GetAccountDetailsAsync();
        }
        public static User GetInfo()
        {
            return user;
        }
    }
}
