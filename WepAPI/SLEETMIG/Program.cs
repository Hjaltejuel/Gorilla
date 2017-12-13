using Entities.RedditEntities;
using System;

namespace SLEETMIG
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
            User user = await rcc.GetAccountDetailsAsync();
            string a = "";
        }

        static async void PostComment()
        {
            IRedditAPIConsumer rcc = new RedditConsumerController();
            //(HttpStatusCode, string) loginResponse = await rcc.LoginToReddit("YAzEEEEEEEES", "12341234");

            //User asd = await rcc.GetAccountDetails();
            Post pretendPost = new Post()
            {
                name = "t3_6q7512"
            };
            var str = await rcc.CreateCommentAsync(pretendPost, "Teeest");

            string a = "";
        }


        static void RefreshToken()
        {
            IRedditAPIConsumer rcc = new RedditConsumerController();
            //(HttpStatusCode, string) loginResponse = await rcc.LoginToReddit("YAzEEEEEEEES", "12341234");

            rcc.RefreshTokenAsync();
            string a = "";
        }
    }
}
    