using Entities.RedditEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UITEST.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UITEST
{
    public sealed partial class CommentControl : UserControl
    {
        private readonly Entities.RedditEntities.Comment currentComment;
        public List<CommentControl> subComments { get; set; }
        int c;
        public CommentControl(Entities.RedditEntities.Comment comment)
        {
            this.InitializeComponent();
            c = 0;
            //Make root comment bordered
            if (comment.depth == 0)
            {
                CommentStackPanel.BorderThickness = new Thickness(1);
                CommentStackPanel.BorderBrush = new SolidColorBrush(Colors.Gray);
                CommentStackPanel.Margin = new Thickness(0, 0, 0, 10);
            }

            if (comment.depth%2==0)
                CommentStackPanel.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            else
                CommentStackPanel.Background = new SolidColorBrush(Color.FromArgb(255, 240, 240, 240));

            currentComment = comment;
            CreateChildComments();
        }

        public void CreateChildComments()
        {
            foreach (Entities.RedditEntities.Comment comment in currentComment.Replies)
            {
                if (comment.body == null)
                {
                    Button b = new Button()
                    {
                        Content = "Klik her for at crashe",
                        Margin = new Thickness(20, 5, 0, 5)
                    };
                    b.Click += B_Click;
                    CommentStackPanel.Children.Add(b);
                }
                else
                    CommentStackPanel.Children.Add(new CommentControl(comment));
            }
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            CommentStackPanel.Children.Remove(b);
            throw new NotImplementedException();
        }

        private void UpvoteButton_Click(object sender, RoutedEventArgs e)
        {
            currentComment.score++;

        }

        private void DownvoteButton_Click(object sender, RoutedEventArgs e)
        {
            currentComment.score--;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
