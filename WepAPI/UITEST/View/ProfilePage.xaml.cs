using UITEST.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UITEST.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page
    {
        private readonly ProfilePageViewModel _vm;

        public ProfilePage()
        {
            this.InitializeComponent();

            _vm = App.ServiceProvider.GetService<ProfilePageViewModel>();

            DataContext = _vm;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await _vm.Initialize();
        }

        private void ListView_SelectionItem(object sender, SelectionChangedEventArgs e)
        {
            Frame.Navigate(typeof(PostPage), e.AddedItems[0]);
        }
    }
}
