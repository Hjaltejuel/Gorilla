using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Entities.RedditEntities;
using Microsoft.Extensions.DependencyInjection;
using UITEST.ViewModel;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UITEST.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreatePostPage : Page
    {
        CreatePostPageViewModel _vm;

        public CreatePostPage()
        {
            _vm = App.ServiceProvider.GetService<CreatePostPageViewModel>();
            this.InitializeComponent();
            _vm.LoadingRingOnOf += SwitchLoadingRingIsActive;
        }
        
        private void SwitchLoadingRingIsActive()
        {
            LoadingRing.IsActive = LoadingRing.IsActive != true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _vm.CurrentSubreddit = e.Parameter as Subreddit;
            base.OnNavigatedTo(e);
        }
    }
}
