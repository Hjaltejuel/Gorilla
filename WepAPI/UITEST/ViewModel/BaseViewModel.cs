using Gorilla.AuthenticationGorillaAPI;
using Gorilla.Model;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UITEST.View;
using Windows.Security.Credentials;

namespace UITEST.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public ICommand GoToHomePageCommand { get; set; }
        public ICommand GoToTrendingPageCommand { get; set; }
        public ICommand GoToDiscoverPageCommand { get; set; }
        public ICommand GoToProfilePageCommand { get; set; }

        
        protected IAuthenticationHelper _helper;
        protected INavigationService _service;


        private WebAccount _account;

        public BaseViewModel(INavigationService service)
        {
            GoToHomePageCommand = new RelayCommand(o => _service.Navigate(typeof(MainPage), o));
            GoToDiscoverPageCommand = new RelayCommand(o => _service.Navigate(typeof(DiscoverPage), o));
            GoToProfilePageCommand = new RelayCommand(o => _service.Navigate(typeof(ProfilePage), o));
            GoToTrendingPageCommand = new RelayCommand(o => _service.Navigate(typeof(TrendingPage), o));
            _service = service;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<WebAccount> Authorize()
        {
            if (_account != null)
            {
                return _account;
            }
            else
            {
                _account = await _helper.SignInAsync();
            }
            return _account;
        }
    }
}
