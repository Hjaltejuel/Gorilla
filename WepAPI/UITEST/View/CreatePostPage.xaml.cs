using UITEST;
using UITEST.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Navigation;
using Entities.RedditEntities;
using Windows.UI.Popups;
using System.Net;

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
            StopLoadingRing();
            _vm.SentSuccesfulEvent += StopLoadingRing;
            _vm.SentSuccesfulEvent += CreatePopUpResponseSuccesful;
            _vm.SentUnsuccesfulEvent += StopLoadingRing;
            _vm.SentUnsuccesfulEvent += CreatePopUpResponseUnsuccesful;
        }

        private void SubmitPostButton_Click(object sender, RoutedEventArgs e)
        {
            var title = TitleText.Text;
            if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title))
            {
                messageDialog = new MessageDialog("A post needs a title");
                messageDialog.ShowAsync();
                return;
            }

            var body = BodyText.Text;
            LoadingRing.IsActive = true;
            _vm.CreateNewPostAsync(title, body);
        }

        private void StopLoadingRing()
        {
            LoadingRing.IsActive = false;
        }

        private void CreatePopUpResponseSuccesful()
        {
            messageDialog = new MessageDialog("Back to subreddit", "Succes");
            messageDialog.Commands.Add(new UICommand("Ok", new UICommandInvokedHandler(BackToSubreddit)));
            messageDialog.ShowAsync();
        }
        private void CreatePopUpResponseUnsuccesful()
        {
            messageDialog = new MessageDialog("Clicking Ok will keep you at the current page", "Could not create post");
            messageDialog.ShowAsync();
        }

        private void BackToSubreddit(IUICommand command)
        {
            this.Frame.GoBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _vm.currentSubreddit = e.Parameter as Subreddit;
            base.OnNavigatedTo(e);
        }

    }
}
