﻿
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Extensions.DependencyInjection;
using UITEST.ViewModel;
using Entities.RedditEntities;
using System.Collections.Generic;
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UITEST.View
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
            
            _vm = App.ServiceProvider.GetService<MainPageViewModel>();

            DataContext = _vm;
            SizeChanged += ChangeListViewWhenSizedChanged;
            _vm.PostsReadyEvent += PostReadyEvent;
            _vm.PostsStartedLoading += StartedLoading;
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
        private void StartedLoading()
        {
            LoadingRing.IsActive = true;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }
        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            LoadingRing.IsActive = true;
            Frame.Navigate(typeof(SubredditPage), args.QueryText);
        }
    }
}
