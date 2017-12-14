using Entities.RedditEntities;
using Microsoft.Extensions.DependencyInjection;
using UITEST.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UITEST.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DiscoverPage : Page
    {
        private readonly DiscoverPageViewModel _vm;
        private Subreddit selectedSubReddit = null;

        public DiscoverPage()
        {
            this.InitializeComponent();

            LoadingRing.IsActive = true;

            _vm = App.ServiceProvider.GetService<DiscoverPageViewModel>();

            DataContext = _vm;

            SizeChanged += ChangeListViewWhenSizedChanged;

            _vm.DiscoverReadyEvent += DiscoverReadyEvent;
        }
        private void ChangeListViewWhenSizedChanged(object sender, SizeChangedEventArgs e)
        {
            DiscoverList.Height = e.NewSize.Height - (commandBar.ActualHeight+75);
        }
        private void DiscoverReadyEvent()
        {
            LoadingRing.IsActive = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _vm.Initialize();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ListView;
            selectedSubReddit = list.SelectedItem as Subreddit;
            _vm.GoToSubRedditPage.Execute(selectedSubReddit);
        }
    }
}
