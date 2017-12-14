
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Core;

using System;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.Security.Authentication.Web.Core;
using Windows.UI.Popups;
using System.Threading.Tasks;
using UITEST.ViewModel;
using Entities.RedditEntities;
using UITEST.View;
using Windows.UI.Xaml.Input;
using Windows.UI.Text;
using System.Collections.Generic;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UITEST
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly MainPageViewModel _vm;
        private List<string> SortTypes;
        private TextBlock NothingFoundTextBlock;

        public MainPage()
        {
            this.InitializeComponent();
            PostsList.Visibility = Visibility.Collapsed;
           
            _vm = App.ServiceProvider.GetService<MainPageViewModel>();
            DataContext = _vm;
           
            SizeChanged += ChangeListViewWhenSizedChanged;
            _vm.PostsReadyEvent += PostReadyEvent;
            SortTypes = new List<string>() { "hot", "new", "rising", "top", "controversial" };
        }
        private void ChangeListViewWhenSizedChanged(object sender, SizeChangedEventArgs e)
        {
            PostsList.Height = e.NewSize.Height - (commandBar.ActualHeight+75);
        }
        private void PostReadyEvent()
        {
            LoadingRing.IsActive = false;
        }

        private void Title_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            Post post = btn.DataContext as Post;
            Frame.Navigate(typeof(PostPage), post);
        }

        private void PostVoteButton_Clicked(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var post = btn.DataContext as Post;
            if (btn.Content.Equals("Like"))
            {
                post.score += 1;
                btn.Style = App.Current.Resources["LikeButtonClicked"] as Style;
            }
            else if (btn.Content.Equals("Dislike"))
            {
                post.score -= 1;
                btn.Style = App.Current.Resources["DislikeButtonClicked"] as Style;
            }
        }
        
        private void TextButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.FontWeight = FontWeights.SemiBold;
        }

        private void TextButton_PointerLeaved(object sender, PointerRoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.FontWeight = FontWeights.Normal;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (_vm.subreddit != null)
                PageTitleText.Text = _vm.subreddit.display_name_prefixed;
        }

        private void List_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            PostsList.Visibility = Visibility.Visible;
        }

        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            SearchForSubreddit(args.QueryText);
        }
        private async void SearchForSubreddit(string SubredditToSearchFor)
        {
            LoadingRing.IsActive = true;
            Grid.Children.Remove(NothingFoundTextBlock);
            await _vm.GeneratePosts(SubredditToSearchFor);
            if (_vm.subreddit.display_name == null)
            {
                PageTitleText.Text = "";
                NothingFoundTextBlock = new TextBlock() { Text = $"Nothing Found on r/{SubredditToSearchFor}", FontSize = 50, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center};
                Grid.Children.Add(NothingFoundTextBlock);
                Grid.SetRow(NothingFoundTextBlock, 3);
            }
            else
            {
                PageTitleText.Text = _vm.subreddit.display_name_prefixed;
            }
        }

        private void SubsribeToSubredditButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.SubscribeToSubreddit();
            string s = _vm.subreddit.user_is_subscriber;
        }

        private void SortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadingRing.IsActive = true;
            var comboBox = sender as ComboBox;
            var SortString = comboBox.SelectedItem as string;
            _vm.GeneratePosts(_vm.subreddit.display_name, SortString);
        }
    }
}
