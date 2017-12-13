using Entities.RedditEntities;
using Gorilla.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UITEST.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Text;
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
        private readonly Comment currentComment;
        private TextBox CommentTextBox;
        private RelativePanel InsertCommentPanel;
        private IRedditAPIConsumer redditAPIConsumer;

        public CommentControl(Comment comment)
        {
            redditAPIConsumer = new RedditConsumerController();
            this.InitializeComponent();
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
            CheckNumberOfPointsIsOnlyOne();
            SetUpTimeText();
            CreateChildComments();
        }

        private void CheckNumberOfPointsIsOnlyOne()
        {
            if (currentComment.score == 1)
            {
                PointsOrPoint.Text = "point";
            }
        }

        private void SetUpTimeText()
        {
            TimeText.Text = TimeHelper.CalcCreationDate(currentComment);
        }

        public void CreateChildComments()
        {
            foreach (Comment comment in currentComment.Replies)
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

        private void TextButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.FontWeight = FontWeights.Bold;
        }

        private void TextButton_PointerLeaved(object sender, PointerRoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.FontWeight = FontWeights.SemiBold;
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e)
        {
            var CommentBtn = sender as Button;

            if (InsertCommentPanel == null)
            {
                CreateCommentPanel();
                var CommentExtraTextPanel = CommentBtn.Parent as RelativePanel;
                TextInfoPanel.Children.Add(InsertCommentPanel);
            }
            else
            {
                TextInfoPanel.Children.Remove(InsertCommentPanel);
                InsertCommentPanel = null;
            }
        }

        private void CreateCommentPanel()
        {
            if (InsertCommentPanel != null)
            {
                InsertCommentPanel = null;
            }
            InsertCommentPanel = new RelativePanel() { Margin = new Thickness(0, 40, 0, 0) };
            CommentTextBox = new TextBox()
            {
                Height = 200,
                Width = 600,
                AcceptsReturn = true,
                TextWrapping = TextWrapping.Wrap,
                IsSpellCheckEnabled = true,
                Language = "en-US"
            };

            Button SubmitButton = new Button()
            {
                Content = "Save",
                Margin = new Thickness(0, 10, 10, 0)
            };
            RelativePanel.SetBelow(SubmitButton, CommentTextBox);
            SubmitButton.Click += CommentSaveClick;

            InsertCommentPanel.Children.Add(CommentTextBox);
            InsertCommentPanel.Children.Add(SubmitButton);
        }

        private void CommentSaveClick(object sender, RoutedEventArgs e)
        {
            InsertComment(currentComment);
        }

        private void InsertComment(AbstractCommentable abstractCommentableToCommentOn)
        {
            if (!CommentTextBox.Text.Equals(""))
            {
                var newComment = new Comment()
                {
                    body = CommentTextBox.Text,
                    author = "ASD",
                    depth = currentComment.depth + 1
                };

                redditAPIConsumer.PostCommentAsync(abstractCommentableToCommentOn, newComment.body);

                currentComment.Replies.Insert(0, newComment);

                CommentTextBox.Text = "";
                TextInfoPanel.Children.Remove(InsertCommentPanel);
                InsertCommentPanel = null;

                CommentStackPanel.Children.Insert(1, new CommentControl(newComment));
            }
        }
    }
}
