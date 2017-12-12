using Entities.RedditEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLET_MIG
{
    class Program
    {
        static void Main(string[] args)
        {
            PostComment();
            Console.Read();
        }
        static async void GetAccountDetails()
        {
            IRedditAPIConsumer rcc = new RedditConsumerController();
            User user = await rcc.GetAccountDetails();
            string a = "";
        }

        static async void PostComment()
        {
            IRedditAPIConsumer rcc = new RedditConsumerController();
            //(HttpStatusCode, string) loginResponse = await rcc.LoginToReddit("YAzEEEEEEEES", "12341234");

            //User asd = await rcc.GetAccountDetails();
            var str = await rcc.CreateComment(null, null);

            string a = "";
        }


        static void RefreshToken()
        {
            IRedditAPIConsumer rcc = new RedditConsumerController();
            //(HttpStatusCode, string) loginResponse = await rcc.LoginToReddit("YAzEEEEEEEES", "12341234");

            rcc.RefreshToken();
            string a = "";
        }

    }
}
