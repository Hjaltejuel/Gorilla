using Entities.RedditEntities;
using Gorilla.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UITEST.Model;
using UITEST.View;
using Microsoft.Extensions.DependencyInjection;
namespace UITEST.ViewModel
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public ObservableCollection<Post> Posts { get; private set; }
        

        private Profile currentProfile;

        public Profile CurrentProfile
        {
            get => currentProfile;
            set
            {
                currentProfile = value;
                OnPropertyChanged("CurrentProfile");
            }
        }

        public ICommand GoToPostPageCommand { get; set; }

     

        public ProfilePageViewModel(INavigationService service) : base(service)
        {
            

            Initialize();
        }

        private void Initialize()
        {
            /*
            Posts = new ObservableCollection<Post>
            {
                new Post {Title = "hej", Author = "Raaaasmusss", NumOfVotes = -100, Text = "FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. hej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gid"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
            };
            */
            GetCurrentProfile();
        }

        private async void GetCurrentProfile()
        {
            var redditAPIConsumer = App.ServiceProvider.GetService<IRedditAPIConsumer>();
            var redditUser = await redditAPIConsumer.GetAccountDetailsAsync();
            var subscriptions = await redditAPIConsumer.GetSubscribedSubredditsAsync();
            var unix = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            var time = unix.AddSeconds(redditUser.created);
            CurrentProfile = new Profile()
            {
                Name = "Morten",
                Username = redditUser.name,
                Email = "thereddestroyer@gmail.com",
                AmountOfSubRedditsSubscribedTo = subscriptions.Count,
                JoinDate = time,
                CommentKarma = redditUser.comment_karma,
                LinkKarma = redditUser.link_karma,
                PostCreated = 54,
                PathToProfilePicture = redditUser.icon_img
            }; 
        }

        private async void GetCommentHistory()
        {

        }

        private async void GetPostHistory()
        {

        }
    }
}
