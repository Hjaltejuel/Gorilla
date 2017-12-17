using UI.Lib.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Entities.RedditEntities;
using Windows.UI.Xaml;


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
            InitializeComponent();
            _vm = App.ServiceProvider.GetService<ProfilePageViewModel>();

            DataContext = _vm;
            SizeChanged += ChangeListViewWhenSizedChanged;
            _vm.PostsReadyEvent += PostReadyEvent;
            PostsList.OnNagivated += PostsList_OnNagivated;
            
        }
        private void PostsList_OnNagivated(Post post)
        {
            Frame.Navigate(typeof(PostPage), post);
        }
        private void ChangeListViewWhenSizedChanged(object sender, SizeChangedEventArgs e)
        {
            PostsList.Height = e.NewSize.Height - (commandBar.ActualHeight + 75);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            LoadingRing.IsActive = true;
            await _vm.Initialize();
        }

        private void PostReadyEvent()
        {
            LoadingRing.IsActive = false;
        }
        
    }
}
