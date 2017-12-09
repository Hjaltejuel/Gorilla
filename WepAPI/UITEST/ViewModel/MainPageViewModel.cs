using Gorilla.AuthenticationGorillaAPI;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UITEST.Model;

namespace UITEST.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {

        public ObservableCollection<Post> Posts { get; set; }
        private readonly IAuthenticationHelper _helper;
        private readonly ISubredditRepository _repository;
        public MainPageViewModel(ISubredditRepository repository, IAuthenticationHelper helper)
        {
            
            Initialize();
            _repository = repository;
            _helper = helper;

        }

        public async void Initialize()
        {
            
                
            var _account = await _helper.SignInAsync();
            if (_account != null)
            {
                var subreddits = await _repository.ReadAsync();
                Posts = new ObservableCollection<Post>
            {
                new Post {Title = "hej", Author = "Raaaasmusss", NumOfVotes = -100, Text = "FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. FY for saaaaataan. hej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gidhej hej dig gid"
                , Comments = new ObservableCollection<Comment>()
                    {
                        new Comment() {Author = "Jens", Text="Jaa jeg er lidt ening", NumOfVotes = 6, Comments = new ObservableCollection<Comment>()
                        {
                            new Comment(){Author = "Hjalte", Text = "Det er jeg sku ikke", Comments = new ObservableCollection<Comment>()
                            {
                                new Comment(){Author = "Morten", Text = "Jeg kan bare godt lide flutes"},
                                new Comment(){Author = "Jens", Text = "Hjalte du tager helt fejl din røde hund"}
                            }},
                            new Comment(){Author = "Jakob", Text = "Fuldstændig enig med dig Jens!", Comments = new ObservableCollection<Comment>()
                            {
                                new Comment(){Author = "Jens", Text = "Tak Jakob, ham der hjalte er sku lidt skør"},
                                new Comment(){Author = "Jakob", Text = "Haha ja det må man nok sige xD"}
                            }}
                        }},
                        new Comment() {Author = "Rasmus", Text = "Jeg har også en post i kan se på" },
                         new Comment() {Author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                          new Comment() {Author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                           new Comment() {Author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                            new Comment() {Author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                             new Comment() {Author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                              new Comment() {Author = "Rasmus", Text = "Jeg har også en post i kan se på"}, new Comment() {Author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                               new Comment() {Author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                                new Comment() {Author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                                 new Comment() {Author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                                  new Comment() {Author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                                   new Comment() {Author = "Rasmus", Text = "Jeg har også en post i kan se på"},
                    }
                },
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
                new Post {Title = "VIld Nice", Author = "Maads", NumOfVotes = 121, Text = "nice nice nicenicenicenci"},
            };
            }
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
    }
}
