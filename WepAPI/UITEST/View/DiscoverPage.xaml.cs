using Entities.RedditEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UITEST.Model;
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
            _vm = new DiscoverPageViewModel()
            {
                GoToHomePageCommand = new RelayCommand(o => Frame.Navigate(typeof(MainPage))),
                GoToDiscoverPageCommand = new RelayCommand(o => Frame.Navigate(typeof(DiscoverPage))),
                GoToProfilePageCommand = new RelayCommand(o => Frame.Navigate(typeof(ProfilePage))),
                GoToTrendingPageCommand = new RelayCommand(o => Frame.Navigate(typeof(TrendingPage))),
                GoToSubRedditPage = new RelayCommand(o => Frame.Navigate(typeof(MainPage)))
            };

            DataContext = _vm;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ListView;
            selectedSubReddit = list.SelectedItem as Subreddit;
            _vm.GoToSubRedditPage.Execute(selectedSubReddit);
        }
    }
}
