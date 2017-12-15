using Entities.RedditEntities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UITEST.View
{
    public sealed partial class CommentsListControl : UserControl
    {
        public CommentsListControl()
        {
            this.InitializeComponent();
        }
        public ObservableCollection<Comment> Comments
        {
            get { return (ObservableCollection<Comment>)GetValue(CommentsProperty); }
            set { SetValue(CommentsProperty, value); }
        }
        public static readonly DependencyProperty CommentsProperty = DependencyProperty.Register
        (
            "Comments",
            typeof(string),
            typeof(CommentsListControl),
            new PropertyMetadata(string.Empty)
        );

        private async void CommentSaveClick(object sender, RoutedEventArgs e)
        {
            //await InsertComment(currentComment);
        }

        private void TextButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {

        }

        private void TextButton_PointerLeaved(object sender, PointerRoutedEventArgs e)
        {
        }

        private async void CommentButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private async void UpvoteButton_Click(object sender, RoutedEventArgs e)
        {
        }
        private async void DownvoteButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
