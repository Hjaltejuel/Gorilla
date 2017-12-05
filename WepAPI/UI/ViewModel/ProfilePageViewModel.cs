using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UITEST.Model;

namespace UITEST.ViewModel
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public ObservableCollection<Post> Posts { get; private set; }

        public Profile CurrentProfile { get; set; }

        public ICommand GoToPostPageCommand { get; set; }

        public ProfilePageViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            Posts = new ObservableCollection<Post>
            {
                new Post {Title = "hej", Author = "Raaaasmusss", NumOfComments = 100, NumOfVotes = -100, Text = "FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. hej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gid"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfComments = 221, NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfComments = 221, NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfComments = 221, NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
            };

            CurrentProfile = new Profile() { Name = "Morten", Username = "RedRabbitRasmusRaarup", Email = "thereddestroyer@gmail.com", AmountOfSubRedditsSubscribedTo = 12, JoinDate = new DateTime(2015, 02, 15), KarmaGiven = 201, KarmaRecieved = 635, PostCreated = 54, PathToProfilePicture = "/MockUpPictures/profilePicture.jpg" };
        }
    }
}
