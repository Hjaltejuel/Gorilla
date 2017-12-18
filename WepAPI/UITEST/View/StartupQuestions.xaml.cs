using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UITEST.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartupQuestions : Page
    {
        public StartupQuestions()
        {
            this.InitializeComponent();
        }

        private void nextpage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ChooseYourCategories));
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            Storyboard fadeIn = this.Resources["FadeIn"] as Storyboard;
            fadeIn.Begin();
           
        }
    }
}
