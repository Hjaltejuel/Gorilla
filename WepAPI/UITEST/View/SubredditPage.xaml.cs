using UI.Lib.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Entities.RedditEntities;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UITEST.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SubredditPage : Page
    {
        public SubredditPage()
        {
            InitializeComponent();
            _vm = App.ServiceProvider.GetService<SubredditPageViewModel>();
            DataContext = _vm;
            SizeChanged += ChangeListViewWhenSizedChanged;
            PostsList.OnNagivated += PostsList_OnNagivated;
            _vm.LoadSwitch += LoadingRingSwitch;
        }

        private readonly SubredditPageViewModel _vm;

        private void PostsList_OnNagivated(Post post)
        {
            Frame.Navigate(typeof(PostPage), post);
        }

        private void ChangeListViewWhenSizedChanged(object sender, SizeChangedEventArgs e)
        {
            PostsList.Height = e.NewSize.Height - (commandBar.ActualHeight + 75);
        }
        private void LoadingRingSwitch()
        {
            LoadingRing.IsActive = !LoadingRing.IsActive;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var navigationParameterTuple = e.Parameter as (Subreddit, string)?;
            if (navigationParameterTuple == null) { return; }
            var subreddit = navigationParameterTuple?.Item1;
            var queryString = navigationParameterTuple?.Item2;   

            if (!string.IsNullOrEmpty(subreddit?.name))
            {
                _vm._Subreddit = subreddit;
                _vm.Posts = subreddit.posts;
                _vm.SubredditName = subreddit.display_name_prefixed;
            }
            else
            {
                NothingFoundTextBlock.Visibility = Visibility.Visible;
                NothingFoundTextBlock.Text = $"Nothing Found on r/{queryString}";
                SubsribeToSubredditButton.Visibility = Visibility.Collapsed;
                PostsList.Visibility = Visibility.Collapsed;
                SortBy.Visibility = Visibility.Collapsed;
                CreatePostButton.Visibility = Visibility.Collapsed;
                PageTitleText.Visibility = Visibility.Collapsed;
            }
        }

        private async void SearchBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var QueryString = sender.Text;
            await _vm.SearchQuerySubmitted(QueryString);
        }

        private async void SearchBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing,
            // otherwise assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen.
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput) return;
            //Set the ItemsSource to be your filtered dataset
            //sender.ItemsSource = dataset;
            var data = await _vm.GetFiltered(sender.Text);
            sender.ItemsSource = data;
        }

        private void SearchBox_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = (string)args.SelectedItem;
        }

        private void SortBy_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var obj = sender as ComboBox;
            var selectedValue = obj.SelectedValue as string;
            _vm.SortBy(selectedValue);
        }
    }
}
