using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Microsoft.Extensions.DependencyInjection;
using UI.Lib.ViewModel;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace UITEST.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private readonly LoginPageViewModel _vm;
        public LoginPage()
        {
            _vm = App.ServiceProvider.GetService<LoginPageViewModel>();
            DataContext = _vm;
            InitializeComponent();
        }
        public void HasAuthenticated()
        {
            Frame.Navigate(typeof(StartupQuestions));
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            Storyboard fadeIn = Resources["FadeIn"] as Storyboard;
            fadeIn.Begin();
            var navParam = e.Parameter as string;
            if (navParam.Equals("logout"))
            {
                LoginButton.Visibility = Visibility.Visible;
                _vm.LogOut();
            }
            else
            {
                LoginButton.Visibility = Visibility.Collapsed;
                await AuthenticateUser();
            }
        }

        private async Task AuthenticateUser()
        {
            if (!await _vm.BeginAuthentication())
            {
                LoginButton.Visibility = Visibility.Visible;
            }
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            await AuthenticateUser();
        }
    }
}
