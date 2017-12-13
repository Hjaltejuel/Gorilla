using UITEST;
using UITEST.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Gorilla.View
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
            StopLoadingRing();
            _vm.PostSentEvent += StopLoadingRing;
        }

        private void SubmitPostButton_Click(object sender, RoutedEventArgs e)
        {
            var title = TitleText.Text;
            if (title.Equals(""))
                return;

            var body = BodyText.Text;
            LoadingRing.IsActive = true;
            _vm.CreateNewPostAsync(title, body);
        }

        private void StopLoadingRing()
        {
            LoadingRing.IsActive = false;
        }
        
    }
}
