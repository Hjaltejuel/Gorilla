using UITEST;
using UITEST.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Navigation;
using Entities.RedditEntities;
using Windows.UI.Popups;
using System.Net;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Gorilla.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreatePostPage : Page
    {
        CreatePostPageViewModel _vm;
        private MessageDialog messageDialog;

        public CreatePostPage()
        {
            _vm = App.ServiceProvider.GetService<CreatePostPageViewModel>();
            this.InitializeComponent();
            _vm.LoadingRingOnOf += SwitchLoadingRingIsActive;
        }
        
        private void SwitchLoadingRingIsActive()
        {
            LoadingRing.IsActive = LoadingRing.IsActive == true ? false : true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _vm.currentSubreddit = e.Parameter as Subreddit;
            base.OnNavigatedTo(e);
        }
    }
}
