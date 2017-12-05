using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UITEST.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public ICommand GoToHomePageCommand { get; set; }
        public ICommand GoToTrendingPageCommand { get; set; }
        public ICommand GoToDiscoverPageCommand { get; set; }
        public ICommand GoToProfilePageCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
