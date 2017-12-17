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
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Storyboard fadeIn = Resources["FadeIn"] as Storyboard;
            fadeIn.Begin();
            var navParam = e.Parameter as string;
            if (navParam.Equals("logout"))
            {
                _vm.LogOut();
            }
            _vm.BeginAuthentication();
        }
    }
}
