﻿
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


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UITEST
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly MainPageViewModel _vm;

        public MainPage()
        {
            this.InitializeComponent();
            PostsList.Visibility = Visibility.Collapsed;
            LoadingRing.IsActive = true;
           
            _vm = App.ServiceProvider.GetService<MainPageViewModel>();

           
            DataContext = _vm;
            _vm.PostsReadyEvent += PostReadyEvent;
            SizeChanged += ResizeListViewHeight;
        }

        private void PostReadyEvent()
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
            Frame.Navigate(typeof(PostPage), e.AddedItems[0]);
        }

        private void ResizeListViewHeight(object sender, SizeChangedEventArgs e)
        {
            //PostsList.Height = e.NewSize.Height - (commandBar.Height + PageTitleText.Height + HorizontalSplitter.Height);
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

        private void RelativePanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var a = "";
        }

        private void List_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            PostsList.Visibility = Visibility.Visible;
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
