
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Extensions.DependencyInjection;

using System;
using UITEST.ViewModel;
using Entities.RedditEntities;
using Windows.UI.Xaml.Input;
using Windows.UI.Text;
using System.Collections.Generic;
using UITEST.View;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UITEST.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly MainPageViewModel _vm;
        private List<string> SortTypes;

        public MainPage()
        {
            this.InitializeComponent();
            
            _vm = App.ServiceProvider.GetService<MainPageViewModel>();

            DataContext = _vm;
            SizeChanged += ChangeListViewWhenSizedChanged;
            _vm.PostsReadyEvent += PostReadyEvent;
            SortTypes = new List<string>() { "hot", "new", "rising", "top", "controversial" };
            PostsList.OnNagivated += PostsList_OnNagivated;
        }

        private void PostsList_OnNagivated(Post post)
        {
            Frame.Navigate(typeof(PostPage), post);
        }

        private void ChangeListViewWhenSizedChanged(object sender, SizeChangedEventArgs e)
        {
            PostsList.Height = e.NewSize.Height - (commandBar.ActualHeight+75);
        }
        private void PostReadyEvent()
        {
            LoadingRing.IsActive = false;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (_vm._Subreddit != null)
                PageTitleText.Text = _vm._Subreddit.display_name_prefixed;
        }
        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            LoadingRing.IsActive = true;
            Frame.Navigate(typeof(SubredditPage), args.QueryText);
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
