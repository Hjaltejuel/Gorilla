using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using UITEST.View;
using Windows.Security.Credentials;
using UITEST.Authentication.GorillaAuthentication;
using UITEST.Model;

namespace UITEST.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public ICommand GoToHomePageCommand { get; set; }
        public ICommand GoToDiscoverPageCommand { get; set; }
        public ICommand GoToProfilePageCommand { get; set; }
        public ICommand LogOutPageCommand { get; set; }


        protected IAuthenticationHelper Helper;
        protected INavigationService Service;


        private WebAccount _account;

        protected BaseViewModel(INavigationService service)
        {
            GoToHomePageCommand = new RelayCommand(o => Service.Navigate(typeof(MainPage), o));
            GoToDiscoverPageCommand = new RelayCommand(o => Service.Navigate(typeof(DiscoverPage), o));
            GoToProfilePageCommand = new RelayCommand(o => Service.Navigate(typeof(ProfilePage), o));
            LogOutPageCommand = new RelayCommand(o => Service.Navigate(typeof(LoginPage), "logout"));
            Service = service;
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
                _account = await Helper.SignInAsync();
            }
            return _account;
        }
    }
}
