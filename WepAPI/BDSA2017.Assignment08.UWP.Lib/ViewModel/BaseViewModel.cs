using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

using Windows.Security.Credentials;
using UI.Lib.Authentication.GorillaAuthentication;
using UI.Lib.Model;

using System;

namespace UI.Lib.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public ICommand GoToHomePageCommand { get; set; }
        public ICommand GoToDiscoverPageCommand { get; set; }
        public ICommand GoToProfilePageCommand { get; set; }
        public ICommand LogOutPageCommand { get; set; }

        public static Type MainPage { get; set; }
        public static Type DiscoverPage { get; set; }
        public static Type ProfilePage { get; set; }
        public static Type LoginPage { get; set; }
        public static Type SubredditPage { get; set; }
        public static Type CreatePostPage { get; set; }
        public static Type StartupQuestionsPage { get; set; }


        protected IAuthenticationHelper _gorillaAuthHelper;
        protected INavigationService Service;


        private WebAccount _account;

        protected BaseViewModel(INavigationService service)
        {
            
            GoToHomePageCommand = new RelayCommand(o => Service.Navigate(MainPage, o));
            GoToDiscoverPageCommand = new RelayCommand(o => Service.Navigate(DiscoverPage, o));
            GoToProfilePageCommand = new RelayCommand(o => Service.Navigate(ProfilePage, o));
            LogOutPageCommand = new RelayCommand(o => Service.Navigate(LoginPage, "logout"));
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
                _account = await _gorillaAuthHelper.SignInAsync();
                if(_account == null)
                {
                    await Authorize();
                }
            }
            return _account;
        }
    }
}
