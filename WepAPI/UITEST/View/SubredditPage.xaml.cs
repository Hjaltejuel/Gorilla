using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UITEST.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
            this.InitializeComponent();
            _vm = App.ServiceProvider.GetService<MainPageViewModel>();
            DataContext = _vm;
            SizeChanged += ChangeListViewWhenSizedChanged;
            _vm.PostsReadyEvent += PostReadyEvent;
            SortTypes = new List<string>() { "hot", "new", "rising", "top", "controversial" };
            PostsList.OnNagivated += PostsList_OnNagivated;
        }

        private readonly MainPageViewModel _vm;
        private List<string> SortTypes;
        private TextBlock NothingFoundTextBlock;

        private void PostsList_OnNagivated(Post post)
        {
            Frame.Navigate(typeof(PostPage), post);
        }

        private void ChangeListViewWhenSizedChanged(object sender, SizeChangedEventArgs e)
        {
            PostsList.Height = e.NewSize.Height - (commandBar.ActualHeight + 75);
        }
        private void PostReadyEvent()
        {
            LoadingRing.IsActive = false;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var SubredditSearchString = e.Parameter as string;
            ShowSubreddit(SubredditSearchString);
        }
        private async void ShowSubreddit(string SubredditSearchString)
        {
            await _vm.GeneratePosts(SubredditSearchString);

            if (_vm._Subreddit == null || _vm._Subreddit.name == null)
            {
                NothingFoundTextBlock = new TextBlock() { Text = $"Nothing Found on r/{SubredditSearchString}", FontSize = 50, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                _Grid.Children.Add(NothingFoundTextBlock);
                Grid.SetRow(NothingFoundTextBlock, 3);
            }
        }
        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            SearchForSubreddit(args.QueryText);
        }
        private async void SearchForSubreddit(string SubredditToSearchFor)
        {
            LoadingRing.IsActive = true;
            _Grid.Children.Remove(NothingFoundTextBlock);
            await _vm.GeneratePosts(SubredditToSearchFor);
            if (_vm._Subreddit.display_name == null)
            {
                PageTitleText.Text = "";
                NothingFoundTextBlock = new TextBlock() { Text = $"Nothing Found on r/{SubredditToSearchFor}", FontSize = 50, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                _Grid.Children.Add(NothingFoundTextBlock);
                Grid.SetRow(NothingFoundTextBlock, 3);
            }
            else
            {
                PageTitleText.Text = _vm._Subreddit.display_name_prefixed;
            }
        }

        private void SubsribeToSubredditButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.SubscribeToSubreddit();
            string s = _vm._Subreddit.user_is_subscriber;
        }

        private void SortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadingRing.IsActive = true;
            var comboBox = sender as ComboBox;
            var SortString = comboBox.SelectedItem as string;
            _vm.GeneratePosts(_vm._Subreddit.display_name, SortString);
        }
    }
}
