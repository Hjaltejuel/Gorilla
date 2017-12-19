using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Entities.RedditEntities;
using Microsoft.Extensions.DependencyInjection;
using UI.Lib.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace UITEST.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DiscoverPage : Page
    {
        private readonly DiscoverPageViewModel _vm;
        private Subreddit _selectedSubReddit;

        public DiscoverPage()
        {
            InitializeComponent();
            LoadingRing.IsActive = true;

            _vm = App.ServiceProvider.GetService<DiscoverPageViewModel>();

            DataContext = _vm;
            SizeChanged += ChangeListViewWhenSizedChanged;
            _vm.DiscoverReadyEvent += DiscoverReadyEvent;
            _vm.NoElementsEvent += NoElementsEvent;
        }
        private void ChangeListViewWhenSizedChanged(object sender, SizeChangedEventArgs e)
        {
            DiscoverList.Height = e.NewSize.Height - (CommandBar.ActualHeight+75);
        }

        public void NoElementsEvent()
        {
            DiscoverList = null;

            var block = new TextBlock
            {
                Height = 100,
                Width = 200,
                Text = "No User preference was found"
            };
            Grid.SetRow(block, 3);
            GridPanel.Children.Add(block);
        }

        private void DiscoverReadyEvent()
        {
            LoadingRing.IsActive = false;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await _vm.Initialize();
        }
        
        private async void DiscoverList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ListView;
            _selectedSubReddit = list.SelectedItem as Subreddit;
            if (_selectedSubReddit == null)
            {
                return;
            }
            var subredditName = _selectedSubReddit.display_name_prefixed;
            _selectedSubReddit = await _vm.GetSubredditPosts(_selectedSubReddit); //Update subreddit with posts
            if (_selectedSubReddit != null)
            {
                Frame.Navigate(typeof(SubredditPage), (_selectedSubReddit, subredditName));
            }
        }
    }
}
