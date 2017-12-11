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

namespace UITEST.ViewModel
{
    public class DiscoverPageViewModel : BaseViewModel
    {
        public ObservableCollection<SubReddit> SubReddits { get; private set; }

        public ICommand GoToSubRedditPage { get; set; }


        public DiscoverPageViewModel(INavigationService service) :base(service)
        {
          
            GoToSubRedditPage = new RelayCommand(o => _service.Navigate(typeof(MainPage), o));
            Initialize();
        }

        private void Initialize()
        {
            SubReddits = new ObservableCollection<SubReddit>
            {
                new SubReddit {Name = "Destiny 2", PathToPicture = "/MockUpPictures/Destiny2Banner.png"},
                new SubReddit {Name = "Memes", PathToPicture = ""},
                new SubReddit {Name = "Starcraft", PathToPicture = ""},
                new SubReddit {Name = "Cars", PathToPicture = "/MockUpPictures/CarsBanner.png"}
            };
        }
    }
}
