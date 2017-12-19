using Entities.RedditEntities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Entities.GorillaEntities;
using UI.Lib.Misc;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;
using UI.Lib.ViewModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UITEST.View
{
    public sealed partial class CommentControl
    {
        private readonly CommentViewModel _vm;
        public readonly Comment _currentComment;
        private bool _isLiked;
        private bool _isDisliked;

        public CommentControl(Comment comment)
        {
            InitializeComponent();
            _currentComment = comment;
            _vm = App.ServiceProvider.GetService<CommentViewModel>();

            //If the comment is a 'more' type
            if (comment?.body == null)
            {
                CommentRelativePanel.Visibility = Visibility.Collapsed;
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
                if (comment.depth % 2 == 0)
                {
                    if (CommentStackPanel != null)
                        CommentStackPanel.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                }
                else
                    CommentStackPanel.Background = new SolidColorBrush(Color.FromArgb(255, 240, 240, 240));

                CheckNumberOfPointsIsOnlyOne();
                SetUpTimeText();
                CreateChildComments();
            }
        }

        private void CheckNumberOfPointsIsOnlyOne()
        {
            if (_currentComment.score == 1)
            {
                PointsOrPoint.Text = "point";
            }
        }

        private void SetUpTimeText()
        {
            TimeText.Text = TimeHelper.CalcCreationDate(_currentComment);
        }

        public void CreateChildComments()
        {
            foreach (Comment comment in _currentComment.Replies)
            {
                CommentStackPanel.Children.Add(new CommentControl(comment));
            }
        }

        private async void B_Click(object sender, RoutedEventArgs e)
        {
            var parentPanel = this.Parent as StackPanel;
            await LoadMoreComments();
            parentPanel?.Children.Remove(this);
        }

        private async Task LoadMoreComments()
        {
            if (Parent is StackPanel parentPanel)
            {
                var _comment = parentPanel.DataContext as Comment;
                var parentComment = _comment;
                var postId = parentComment.link_id;
                var children = _currentComment.children;

                if (postId != null && children.Length != 0)
                {
                    var list = await _vm.GetChildComments(postId, children, _currentComment);
                    foreach (var comment in list)
                    {
                        parentPanel.Children.Add(new CommentControl(comment));
                    }
                }
            }
        }

        private async void UpvoteButton_Click(object sender, RoutedEventArgs e)
        {
            await CommentLikedAsync();
        }

        private async void DownvoteButton_Click(object sender, RoutedEventArgs e)
        {
           await CommentDislikedAsync();
        }

        private int GetNewVoteDirection(int voteDirection) // 1 = upvote | -1 = downvote
        {
            var currentDirection = 0;
            if (_isLiked)
            {
                currentDirection = 1;
            }
            else if (_isDisliked)
            {
                currentDirection = -1;
            }
            if (currentDirection + voteDirection == 0)
            {
                return voteDirection;
            }
            return (currentDirection + voteDirection) % 2;
        }

        private void UpdateVoteUi(int voteDirection) // 1 = upvote | -1 = downvote
        {
            Downvote.Style = _downvoteNotClickedStyle;
            Upvote.Style = _upvoteNotClickedStyle;
            if (voteDirection == 1)
            {
                Upvote.Style = _upvoteClickedStyle;
            }
            else if (voteDirection == -1)
            {
                Downvote.Style = _downvoteClickedStyle;
            }

            _currentComment.score += voteDirection;
            if (_isLiked)
                if (voteDirection == 0 || voteDirection == -1)
                    _currentComment.score -= 1;

                else if (_isDisliked)
                    if (voteDirection == 0 || voteDirection == 1)
                        _currentComment.score += 1;

            PointsTextBlock.Text = _currentComment.score.ToString();

            _isLiked = (voteDirection == 1);
            _isDisliked = (voteDirection == -1);
        }

        //Hvor skal det her stå? vi har ikke en viewmodel
        //TODO hvis vi ikke kan få observer pattern til at virke kan vi slette de der currentcomment.score - og + statements
        public async Task CommentLikedAsync()
        {
            var newDirection = GetNewVoteDirection(1);
            UpdateVoteUi(newDirection);
            await _vm.LikeCommentableAsync(_currentComment, newDirection);
        }

        public async Task CommentDislikedAsync()
        {
            var newDirection = GetNewVoteDirection(-1);
            UpdateVoteUi(newDirection);
            await _vm.LikeCommentableAsync(_currentComment, newDirection);
        }

        //Grimt i know.. what to do? det er et midlertidligt workaround
        private readonly Style _upvoteClickedStyle = Application.Current.Resources["LikeButtonClicked"] as Style;
        private readonly Style _upvoteNotClickedStyle = Application.Current.Resources["LikeButton"] as Style;
        private readonly Style _downvoteClickedStyle = Application.Current.Resources["DislikeButtonClicked"] as Style;
        private readonly Style _downvoteNotClickedStyle = Application.Current.Resources["DislikeButton"] as Style;

        private void TextButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Button btn) btn.FontWeight = FontWeights.Bold;
            _vm.SetHandCursor();
        }

        private void TextButton_PointerLeaved(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Button btn) btn.FontWeight = FontWeights.SemiBold;
            _vm.SetArrowCursor();
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e)
        {
            for (var i = 0; i < CommentStackPanel.Children.Count; i++)
            {
                var child = CommentStackPanel.Children[i];
                if (child is CommentPanel)
                {
                    CommentStackPanel.Children.RemoveAt(i);
                    return;
                }
            }

            var commentPanel = new CommentPanel();
            CommentStackPanel.Children.Insert(1, commentPanel);
            commentPanel.OnCommentCreated += CommentPanel_OnCommentCreated;
        }

        private async void CommentPanel_OnCommentCreated(string commentBody)
        {
            var newComment = await _vm.CreateComment(_currentComment, commentBody);
            if (newComment == null) return;
            _currentComment.Replies.Insert(0, newComment);
            CommentStackPanel.Children.Insert(1, new CommentControl(newComment));

        }
    }
}