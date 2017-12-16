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
            var subredditSearchString = e.Parameter as string;
            ShowSubreddit(subredditSearchString);
        }
        private async void ShowSubreddit(string subredditSearchString)
        {
            await _vm.GeneratePosts(subredditSearchString);
            SubsribeToSubredditButton.Visibility = Visibility.Visible;
            if (_vm._Subreddit?.name == null)
            {
                NothingFoundTextBlock.Visibility = Visibility.Visible;
                NothingFoundTextBlock.Text = $"Nothing Found on r/{subredditSearchString}";
                SubsribeToSubredditButton.Visibility = Visibility.Collapsed;
                PostsList.Visibility = Visibility.Collapsed;
                SortBy.Visibility = Visibility.Collapsed;
                CreatePostButton.Visibility = Visibility.Collapsed;
                PageTitleText.Visibility = Visibility.Collapsed;
            }
        }

        private async void SubsribeToSubredditButton_Click(object sender, RoutedEventArgs e)
        {
            await _vm.SubscribeToSubreddit();
        }

        private async void SortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var SortString = comboBox.SelectedItem as string;
            await _vm.GeneratePosts(_vm._Subreddit.display_name, SortString);
        }
    }
}
