using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Microsoft.Extensions.DependencyInjection;
using UITEST.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace UITEST.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            var vm = App.ServiceProvider.GetService<LoginPageViewModel>();
            DataContext = vm;
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Storyboard fadeIn = Resources["FadeIn"] as Storyboard;
            fadeIn.Begin();
        }
    }
}
