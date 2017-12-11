using Entities.RedditEntities;
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
    public class DiscoverPageViewModel : BaseViewModel
    {
        public ObservableCollection<Subreddit> SubReddits { get; private set; }

        public ICommand GoToSubRedditPage { get; set; }

        public DiscoverPageViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            SubReddits = new ObservableCollection<Subreddit>
            {
                new Subreddit {title = "Destiny 2", banner_img = "/MockUpPictures/Destiny2Banner.png"},
                new Subreddit {title = "Memes", banner_img = ""},
                new Subreddit {title = "Starcraft", banner_img = ""},
                new Subreddit {title = "Cars", banner_img = "/MockUpPictures/CarsBanner.png"}
            };
        }
    }
}
