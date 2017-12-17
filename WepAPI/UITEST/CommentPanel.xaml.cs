using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Entities.RedditEntities;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UITEST
{
    public sealed partial class CommentPanel : UserControl
    {
        public delegate void CommentCreatedEvent(string commentBody);
        public event CommentCreatedEvent OnCommentCreated;
        public CommentPanel()
        {
            this.InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var commentBody = CommentTextBox.Text;
            if (string.IsNullOrEmpty(commentBody) || string.IsNullOrWhiteSpace(commentBody))
            {
                ErrorText.Text = "We need something in the textbox";
                ErrorText.Visibility = Visibility.Visible;
            }
            else
            {
                (this.Parent as Panel)?.Children.Remove(this);
                OnCommentCreated?.Invoke(commentBody);
                //Build comment object
                //var utcNow = (int)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;
                //var newComment = new Comment()
                //{
                //    body = commentText,
                //    author = UserFactory.GetInfo().name,
                //    created_utc = utcNow
                //};
                ////Send to listener
                //(this.Parent as Panel)?.Children.Remove(this);
                //OnCommentCreated?.Invoke(newComment);
            }
        }
    }
}
