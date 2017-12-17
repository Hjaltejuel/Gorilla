using System.Threading.Tasks;
using Entities.RedditEntities;
using UI.Lib.Model.RedditRestInterfaces;

namespace UI.Lib.Model
{
    public static class UserFactory
    {

        private static User _user;

        public static async Task Initialize(IRedditApiConsumer consumer)
        {
            _user = (await consumer.GetAccountDetailsAsync()).Item2;
        }
        public static User GetInfo()
        {
            return _user;
        }
    }
}
