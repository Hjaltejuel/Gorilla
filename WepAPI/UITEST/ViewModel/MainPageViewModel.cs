using Entities.RedditEntities;
using Gorilla.AuthenticationGorillaAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UITEST.Model;

namespace UITEST.ViewModel
{
    public class MainPageViewModel : BaseViewModel, INotifyPropertyChanged
    {
        IRedditAPIConsumer APIConsumer;
        public ObservableCollection<Post> Posts { get; set; }
        private IAuthenticationHelper _helper;
        public MainPageViewModel()
        {
            
            Initialize();
        }

        public async void GeneratePosts()
        {
            APIConsumer = new RedditConsumerController();
            Subreddit subreddit = await APIConsumer.GetSubredditAsync("AskReddit");
            Posts = subreddit.posts;
            OnPropertyChanged("Posts");
        }
        public void Initialize()
        {
            GeneratePosts();
            
            /*
            Posts = new ObservableCollection<Post>
            {
                new Post {title = "hej", author = "Raaaasmusss", NumOfVotes = -100, Text = "FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. hej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gid"
                , Comments = new ObservableCollection<Comment>()
                    {
                        new Comment() {author = "Jens", Text="Jaa jeg er lidt ening", NumOfVotes = 6, Comments = new ObservableCollection<Comment>()
                        {
                            new Comment(){author = "Hjalte", Text = "Det er jeg sku ikke", Comments = new ObservableCollection<Comment>()
                            {
                                new Comment(){author = "Morten", Text = "Jeg kan bare godt lide flutes"},
                                new Comment(){author = "Jens", Text = "Hjalte du tager helt fejl din røde hund"}
                            }},
                            new Comment(){author = "Jakob", Text = "Fuldstændig enig med dig Jens!", Comments = new ObservableCollection<Comment>()
                            {
                                new Comment(){author = "Jens", Text = "Tak Jakob, ham der hjalte er sku lidt skør"},
                                new Comment(){author = "Jakob", Text = "Haha ja det må man nok sige xD"}
                            }}
                        }},
                        new Comment() {author = "Rasmus", Text = "Jeg har også en post i kan se på" },
                         new Comment() {author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                          new Comment() {author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                           new Comment() {author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                            new Comment() {author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                             new Comment() {author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                              new Comment() {author = "Rasmus", Text = "Jeg har også en post i kan se på"}, new Comment() {author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                               new Comment() {author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                                new Comment() {author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                                 new Comment() {author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                                  new Comment() {author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                                   new Comment() {author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                    }
                },
                new Post {title = "VIld Nice", author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {title = "VIld Nice", author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {title = "VIld Nice", author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {title = "VIld Nice", author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"}, 
                new Post {title = "VIld Nice", author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"}, 
                new Post {title = "VIld Nice", author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"}, 
                new Post {title = "VIld Nice", author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"}, 
                new Post {title = "VIld Nice", author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"}, 
                new Post {title = "VIld Nice", author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"}, 
                new Post {title = "VIld Nice", author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"}, 
                new Post {title = "VIld Nice", author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"}, 
                new Post {title = "VIld Nice", author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"}, 
                new Post {title = "VIld Nice", author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"}, 
            };*/
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
    }
}
