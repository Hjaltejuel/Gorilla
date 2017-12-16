using Entities.RedditEntities;
using Gorilla.Model;
using Gorilla.ViewModel;
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

namespace UITEST.View
{
    public sealed partial class CommentControl : UserControl
    {
        CommentViewModel _vm;
        private readonly Comment currentComment;
        private TextBox CommentTextBox;
        private TextBlock errorText;
        private RelativePanel InsertCommentPanel;
        private IRedditAPIConsumer redditAPIConsumer;
        private readonly IRestUserPreferenceRepository _repository;
        private bool IsLiked;
        private bool IsDisliked;
        private Comment comment;
        public ObservableCollection<Post> Comment
        {
            get { return (ObservableCollection<Post>)GetValue(CommentProperty); }
            set { SetValue(CommentProperty, value); }
        }
        public static readonly DependencyProperty CommentProperty = DependencyProperty.Register
        (
            "Comment",
            typeof(string),
            typeof(CommentControl),
            new PropertyMetadata(string.Empty)
        );
        public CommentControl()
        {
            redditAPIConsumer = App.ServiceProvider.GetService<IRedditAPIConsumer>();
            _repository = App.ServiceProvider.GetService<IRestUserPreferenceRepository>();
            this.InitializeComponent();
            _vm = App.ServiceProvider.GetService<CommentViewModel>();
            currentComment = comment;

            //If the comment is a 'more' type
            if (comment.body == null)
            {
                var b = new Button()
                {
                    Content = "Load more comments",
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

                if (currentComment.score == 1)
                {
                    PointsOrPoint.Text = "point";
                }
                TimeText.Text = TimeHelper.CalcCreationDate(currentComment);
                foreach (var comment in currentComment.Replies)
                {
                    //CommentStackPanel.Children.Add(new CommentControl(comment));
                }
            }
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            CommentStackPanel.Children.Remove(b);
            LoadMoreComments();
        }

        private async void LoadMoreComments()
        {
            var parentPanel = this.Parent as StackPanel;
            var parentGrid = parentPanel.Parent as Grid;
            var parentCommentControl = parentGrid.Parent as CommentControl;
            var parentComment = parentCommentControl.currentComment;
            var postID = parentComment.link_id;
            var children = currentComment.children;

            //parentCommentControl.CommentStackPanel.Children.Remove(parentCommentControl);
            if (postID != null && children.Length != 0)
            {
                var list = await redditAPIConsumer.GetMoreComments(postID, children, currentComment.depth);
                foreach (var comment in list)
                {
                    //parentPanel.Children.Add(new CommentControl(comment));
                    //this.InsertMoreComment(comment);
                }
            }
            parentPanel.Children.Remove(this);
        }

        private async void UpvoteButton_Click(object sender, RoutedEventArgs e)
        {
            await CommentLikedAsync();
            await _repository.UpdateAsync(new Entities.UserPreference { Username = UserFactory.GetInfo().name, SubredditName = currentComment.subreddit, PriorityMultiplier = 1 });
        }

        private async void DownvoteButton_Click(object sender, RoutedEventArgs e)
        {
            await CommentDislikedAsync();
            await _repository.UpdateAsync(new Entities.UserPreference { Username = UserFactory.GetInfo().name, SubredditName = currentComment.subreddit, PriorityMultiplier = 1 });
        }
        private int GetNewVoteDirection(int voteDirection) // 1 = upvote | -1 = downvote
        {
            var currentDirection = 0;
            if (IsLiked) { currentDirection = 1; }
            else if (IsDisliked) { currentDirection = -1; }
            if (currentDirection + voteDirection == 0)
            {
                return voteDirection;
            }
            return (currentDirection + voteDirection) % 2;
        }
        private void UpdateVoteUI(int voteDirection) // 1 = upvote | -1 = downvote
        {
            Downvote.Style = DownvoteNotClickedStyle;
            Upvote.Style = UpvoteNotClickedStyle;
            if (voteDirection==1) { Upvote.Style = UpvoteClickedStyle; }
            else if (voteDirection == -1) { Downvote.Style = DownvoteClickedStyle; }
            
            currentComment.score += voteDirection;
            if (IsLiked)
                if (voteDirection == 0 || voteDirection == -1)
                    currentComment.score -= 1;

            else if (IsDisliked)
                if (voteDirection == 0 || voteDirection == 1)
                    currentComment.score += 1;
            
            PointsTextBlock.Text = currentComment.score.ToString();

            IsLiked = (voteDirection == 1);
            IsDisliked = (voteDirection == -1);
        }
        //Hvor skal det her stå? vi har ikke en viewmodel
        //TODO hvis vi ikke kan få observer pattern til at virke kan vi slette de der currentcomment.score - og + statements
        public async Task CommentLikedAsync()
        {
            var newDirection = GetNewVoteDirection(1);
            UpdateVoteUI(newDirection);
            await redditAPIConsumer.VoteAsync(currentComment, newDirection);
        }
        public async Task CommentDislikedAsync()
        {
            var newDirection = GetNewVoteDirection(-1);
            UpdateVoteUI(newDirection);
            await redditAPIConsumer.VoteAsync(currentComment, newDirection);
        }

        //Grimt i know.. what to do? det er et midlertidligt workaround
        private readonly Style UpvoteClickedStyle = App.Current.Resources["LikeButtonClicked"] as Style;
        private readonly Style UpvoteNotClickedStyle = App.Current.Resources["LikeButton"] as Style;
        private readonly Style DownvoteClickedStyle = App.Current.Resources["DislikeButtonClicked"] as Style;
        private readonly Style DownvoteNotClickedStyle = App.Current.Resources["DislikeButton"] as Style;
        private void TextButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Button btn) btn.FontWeight = FontWeights.Bold;
        }

        private void TextButton_PointerLeaved(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Button btn) btn.FontWeight = FontWeights.SemiBold;
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e)
        {
            if (InsertCommentPanel == null)
            {
                CreateCommentPanel();
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

            var submitButton = new Button()
            {
                Content = "Save",
                Margin = new Thickness(0, 10, 10, 0)
            };
            RelativePanel.SetBelow(submitButton, CommentTextBox);
            errorText = new TextBlock() { Visibility = Visibility.Collapsed, Margin = new Thickness(10, 7, 0, 0), FontSize = 14 };
            RelativePanel.SetRightOf(errorText, submitButton);
            RelativePanel.SetBelow(errorText, CommentTextBox);
            RelativePanel.SetAlignVerticalCenterWith(errorText, submitButton);
            submitButton.Click += CommentSaveClick;

            InsertCommentPanel.Children.Add(CommentTextBox);
            InsertCommentPanel.Children.Add(submitButton);
            InsertCommentPanel.Children.Add(errorText);
        }

        private async void CommentSaveClick(object sender, RoutedEventArgs e)
        {
            await InsertComment(currentComment);
        }
        private async Task InsertComment(AbstractCommentable abstractCommentableToCommentOn)
        {
            var text = CommentTextBox.Text;
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                errorText.Text = "We need something in the textbox";
                errorText.Visibility = Visibility.Visible;
            }
            else
            {
                var old = new DateTime(1970, 1, 1);
                var totaltime = DateTime.Now - old;
                var timeInSeconds = (int)totaltime.TotalSeconds;
                var newComment = new Comment()
                {
                    body = CommentTextBox.Text,
                    author = UserFactory.GetInfo().name,
                    depth = currentComment.depth + 1,
                    created_utc = timeInSeconds
                };

                await redditAPIConsumer.CreateCommentAsync(abstractCommentableToCommentOn, newComment.body);

                currentComment.Replies.Insert(0, newComment);

                CommentTextBox.Text = "";
                TextInfoPanel.Children.Remove(InsertCommentPanel);
                InsertCommentPanel = null;

                //CommentStackPanel.Children.Insert(1, new CommentControl(newComment));
                await _repository.UpdateAsync(new Entities.UserPreference { Username = UserFactory.GetInfo().name, SubredditName = currentComment.subreddit, PriorityMultiplier = 3 });
            }
        }
    }
}
