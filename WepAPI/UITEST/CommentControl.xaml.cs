using Entities.RedditEntities;
using Gorilla.Model;
using Microsoft.Extensions.DependencyInjection;
using Model;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UITEST.RedditInterfaces;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

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
        private readonly IRestUserPreferenceRepository _repository; 
        private bool IsLiked;
        private bool IsDisliked;

        public CommentControl(Comment comment)
        {
            redditAPIConsumer = App.ServiceProvider.GetService<IRedditAPIConsumer>();
            _repository = App.ServiceProvider.GetService<IRestUserPreferenceRepository>();
            this.InitializeComponent();

            currentComment = comment;

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

        private async void LoadMoreComments()
        {

            var parentPanel = this.Parent as StackPanel;
            var parentGrid = parentPanel.Parent as Grid;
            var parentCommentControl = parentGrid.Parent as CommentControl;
            var parentComment = parentCommentControl.currentComment;
            
            string postID = parentComment.link_id;

            string[] children = currentComment.children;

            //parentCommentControl.CommentStackPanel.Children.Remove(parentCommentControl);
            if (postID != null && children.Length != 0)
            {
                ObservableCollection<Comment> list = await redditAPIConsumer.GetMoreComments(postID, children, currentComment.depth);
                foreach (Comment comment in list)
                {
                    parentPanel.Children.Add(new CommentControl(comment));
                    //this.InsertMoreComment(comment);
                }
            }
            parentPanel.Children.Remove(this);
            
        }

        private async void UpvoteButton_Click(object sender, RoutedEventArgs e)
        {
            await  CommentLikedAsync();
            await _repository.UpdateAsync(new Entities.UserPreference { Username = UserFactory.GetInfo().name, SubredditName = currentComment.subreddit, PriorityMultiplier = 1 });
           
        }

        private async void DownvoteButton_Click(object sender, RoutedEventArgs e)
        {
            await CommentDislikedAsync();
           await _repository.UpdateAsync(new Entities.UserPreference { Username = UserFactory.GetInfo().name, SubredditName = currentComment.subreddit, PriorityMultiplier = 1 });
            


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

        private async void CommentButton_Click(object sender, RoutedEventArgs e)
        {
            var CommentBtn = sender as Button;

            if (InsertCommentPanel == null)
            {
                await CreateCommentPanel();
                var CommentExtraTextPanel = CommentBtn.Parent as RelativePanel;
                TextInfoPanel.Children.Add(InsertCommentPanel);
            }
            else
            {
                TextInfoPanel.Children.Remove(InsertCommentPanel);
                InsertCommentPanel = null;
            }
        }

        private async Task CreateCommentPanel()
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
            SubmitButton.Click +=  CommentSaveClick;

            InsertCommentPanel.Children.Add(CommentTextBox);
            InsertCommentPanel.Children.Add(SubmitButton);
            InsertCommentPanel.Children.Add(errorText);
        }

        private async void CommentSaveClick(object sender, RoutedEventArgs e)
        {
            await InsertComment(currentComment);
        }


        private void InsertMoreComment(Comment comment)
        {

            string text = comment.body;
            
                currentComment.Replies.Insert(0, comment);

                TextInfoPanel.Children.Remove(InsertCommentPanel);
                InsertCommentPanel = null;
                comment.depth += 3;
                CommentStackPanel.Children.Add(new CommentControl(comment));
                
            
        }

        private async Task InsertComment(AbstractCommentable abstractCommentableToCommentOn)
        {   

            string text = CommentTextBox.Text;
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                errorText.Text = "We need something in the textbox";
                errorText.Visibility = Visibility.Visible;
            }
            else
            {
                var old = new DateTime(1970, 1, 1);
                var totaltime = DateTime.Now - old;
                int timeInSeconds = (int)totaltime.TotalSeconds;
                var newComment = new Comment()
                {
                    body = CommentTextBox.Text,
                    author = "ASD",
                    depth = currentComment.depth + 1,
                    created_utc = timeInSeconds
                };

                await redditAPIConsumer.CreateCommentAsync(abstractCommentableToCommentOn, newComment.body);

               

                currentComment.Replies.Insert(0, newComment);

                CommentTextBox.Text = "";
                TextInfoPanel.Children.Remove(InsertCommentPanel);
                InsertCommentPanel = null;

                CommentStackPanel.Children.Insert(1, new CommentControl(newComment));
                await _repository.UpdateAsync(new Entities.UserPreference { Username = UserFactory.GetInfo().name, SubredditName = currentComment.subreddit, PriorityMultiplier = 3 });
            }
        }
    }
}
