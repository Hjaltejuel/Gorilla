using System.Threading.Tasks;
using Entities.RedditEntities;
using UITEST.Model.RedditRestInterfaces;

namespace UITEST.Model
{
    public static class UserFactory
    {

        private static User _user;

        public static async Task Initialize(IRedditApiConsumer consumer)
        {
            _user = await consumer.GetAccountDetailsAsync();
        }
        public static User GetInfo()
        {
            return _user;
        }
    }
}
