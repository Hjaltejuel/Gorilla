using Entities.RedditEntities;
using System.Collections.ObjectModel;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;    

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236
namespace UITEST.View
{
    public sealed partial class PostViewControl : UserControl
    {
        public delegate void NavigateToPostEvent(Post post);
        public event NavigateToPostEvent OnNagivated;
        public PostViewControl()
        {
            InitializeComponent();
        }
        public ObservableCollection<Post> Posts
        {
            get => (ObservableCollection<Post>)GetValue(PostsProperty);
            set => SetValue(PostsProperty, value);
        }
        public static readonly DependencyProperty PostsProperty = DependencyProperty.Register
        (
            "Posts",
            typeof(string),
            typeof(PostViewControl),
            new PropertyMetadata(string.Empty)
        );
        private void Title_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            Post post = btn.DataContext as Post;
            OnNagivated.Invoke(post);
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
    }
}
