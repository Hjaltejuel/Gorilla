using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Entities.RedditEntities;
using Windows.UI.Core;
using UI.Lib.ViewModel;
using System;


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
            InitializeComponent();
            _vm = App.ServiceProvider.GetService<MainPageViewModel>();
            DataContext = _vm;
        
            SizeChanged += ChangeListViewWhenSizedChanged;
            _vm.MainReadyEvent += LoadingRingSwitch;
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
        private void LoadingRingSwitch()
        {
            LoadingRing.IsActive = false;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }


        
    }
}
