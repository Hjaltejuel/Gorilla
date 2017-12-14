using Entities.RedditEntities;
using Gorilla.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
        private TextBlock errorText;
        private RelativePanel InsertCommentPanel;
        private IRedditAPIConsumer redditAPIConsumer;
        private bool IsLiked;
        private bool IsDisliked;

        public CommentControl(Comment comment)
        {
            redditAPIConsumer = new RedditConsumerController();
            this.InitializeComponent();

            //If the comment is a 'more' type
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
            {
                //Make root comment bordered
                if (comment.depth == 0)
                {
                    CommentStackPanel.BorderThickness = new Thickness(1);
                    CommentStackPanel.BorderBrush = new SolidColorBrush(Colors.Gray);
                    CommentStackPanel.Margin = new Thickness(0, 0, 0, 10);
                }

                if (comment.depth % 2 == 0)
                    CommentStackPanel.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                else
                    CommentStackPanel.Background = new SolidColorBrush(Color.FromArgb(255, 240, 240, 240));

                currentComment = comment;
                CheckNumberOfPointsIsOnlyOne();
                SetUpTimeText();
                CreateChildComments();
            }
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
                CommentStackPanel.Children.Add(new CommentControl(comment));
            }
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            CommentStackPanel.Children.Remove(b);
            LoadMoreComments();
        }

        private void LoadMoreComments()
        {
            string postID = null;
            string[] children = null;
            redditAPIConsumer.GetMoreComments(postID, children);
        }

        private void UpvoteButton_Click(object sender, RoutedEventArgs e)
        {
            CommentLikedAsync();
        }

        private void DownvoteButton_Click(object sender, RoutedEventArgs e)
        {
            CommentDislikedAsync();
        }


        //Hvor skal det her stå? vi har ikke en viewmodel
        //TODO hvis vi ikke kan få observer pattern til at virke kan vi slette de der currentcomment.score - og + statements
        public async Task CommentLikedAsync()
        {
            int direction;

            if (IsLiked)
            {
                currentComment.score -= 1;
                direction = 0;
            }
            else
            {
                if (IsDisliked)
                    currentComment.score += 2;
                else
                    currentComment.score += 1;
                direction = 1;
            }
            IsDisliked = false;
            IsLiked = !IsLiked;
            await redditAPIConsumer.VoteAsync(currentComment, direction);
            LikeSuccesful();
        }

        public async Task CommentDislikedAsync()
        {
            int direction;

            if (IsDisliked)
            {
                currentComment.score += 1;
                direction = 0;
            }
            else
            {
                if (IsLiked)
                    currentComment.score -= 2;
                else
                    currentComment.score -= 1;
                direction = -1;
            }
            IsLiked = false;
            IsDisliked = !IsDisliked;
            await redditAPIConsumer.VoteAsync(currentComment, direction);
            DislikeSuccesful();
        }

        //Grimt i know.. what to do? det er et midlertidligt workaround
        private Style UpvoteClickedStyle = App.Current.Resources["LikeButtonClicked"] as Style;
        private Style UpvoteNotClickedStyle = App.Current.Resources["LikeButton"] as Style;
        private Style DownvoteClickedStyle = App.Current.Resources["DislikeButtonClicked"] as Style;
        private Style DownvoteNotClickedStyle = App.Current.Resources["DislikeButton"] as Style;

        private void LikeSuccesful()
        {
            int votes;
            int.TryParse(PointsTextBlock.Text, out votes);

            if (Upvote.Style.Equals(UpvoteClickedStyle)) {
                Upvote.Style = UpvoteNotClickedStyle;
                PointsTextBlock.Text = (votes - 1).ToString();
            }else{
                if (Downvote.Style.Equals(DownvoteClickedStyle))
                {
                    PointsTextBlock.Text = (votes + 2).ToString();

                }else{
                    PointsTextBlock.Text = (votes + 1).ToString();
                }
                Upvote.Style = UpvoteClickedStyle;
            }
            Downvote.Style = DownvoteNotClickedStyle;
        }

        private void DislikeSuccesful()
        {
            int votes;
            int.TryParse(PointsTextBlock.Text, out votes);

            if (Downvote.Style.Equals(DownvoteClickedStyle))
            {
                Downvote.Style = DownvoteNotClickedStyle;
                PointsTextBlock.Text = (votes + 1).ToString();
            }else
            {
                if (Upvote.Style.Equals(UpvoteClickedStyle))
                    PointsTextBlock.Text = (votes - 2).ToString();
                else {
                    PointsTextBlock.Text = (votes - 1).ToString();
                }
                Downvote.Style = DownvoteClickedStyle;
            }
            Upvote.Style = UpvoteNotClickedStyle;
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
            errorText = new TextBlock() { Visibility = Visibility.Collapsed, Margin = new Thickness(10, 7, 0, 0), FontSize = 14 };
            RelativePanel.SetRightOf(errorText, SubmitButton);
            RelativePanel.SetBelow(errorText, CommentTextBox);
            RelativePanel.SetAlignVerticalCenterWith(errorText, SubmitButton);
            SubmitButton.Click += CommentSaveClick;

            InsertCommentPanel.Children.Add(CommentTextBox);
            InsertCommentPanel.Children.Add(SubmitButton);
            InsertCommentPanel.Children.Add(errorText);
        }

        private void CommentSaveClick(object sender, RoutedEventArgs e)
        {
            InsertComment(currentComment);
        }

        private void InsertComment(AbstractCommentable abstractCommentableToCommentOn)
        {
            string text = CommentTextBox.Text;
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                errorText.Text = "We need something in the textbox";
                errorText.Visibility = Visibility.Visible;
            }
            else
            {
                var newComment = new Comment()
                {
                    body = CommentTextBox.Text,
                    author = "ASD",
                    depth = currentComment.depth + 1
                };

                redditAPIConsumer.CreateCommentAsync(abstractCommentableToCommentOn, newComment.body);

                currentComment.Replies.Insert(0, newComment);

                CommentTextBox.Text = "";
                TextInfoPanel.Children.Remove(InsertCommentPanel);
                InsertCommentPanel = null;

                CommentStackPanel.Children.Insert(1, new CommentControl(newComment));
            }
        }
    }
}
