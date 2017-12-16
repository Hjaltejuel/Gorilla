using Entities.RedditEntities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Entities.GorillaEntities;
using UITEST.Misc;
using UITEST.Model;
using UITEST.Model.GorillaRestInterfaces;
using UITEST.Model.RedditRestInterfaces;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UITEST.View
{
    public sealed partial class CommentControl
    {
        private readonly Comment _currentComment;
        private TextBox _commentTextBox;
        private TextBlock _errorText;
        private RelativePanel _insertCommentPanel;
        private readonly IRedditApiConsumer _redditApiConsumer;
        private readonly IRestUserPreferenceRepository _repository;
        private bool _isLiked;
        private bool _isDisliked;

        public CommentControl(Comment comment)
        {
            _redditApiConsumer = App.ServiceProvider.GetService<IRedditApiConsumer>();
            _repository = App.ServiceProvider.GetService<IRestUserPreferenceRepository>();
            InitializeComponent();
            _currentComment = comment;

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

        private void B_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            CommentStackPanel.Children.Remove(b);
            LoadMoreComments();
        }

        private async void LoadMoreComments()
        {
            var parentPanel = Parent as StackPanel;
            var parentGrid = parentPanel?.Parent as Grid;
            if (parentGrid?.Parent is CommentControl parentCommentControl)
            {
                var parentComment = parentCommentControl._currentComment;
                var postId = parentComment.link_id;
                var children = _currentComment.children;

                //parentCommentControl.CommentStackPanel.Children.Remove(parentCommentControl);
                if (postId != null && children.Length != 0)
                {
                    var list = await _redditApiConsumer.GetMoreComments(postId, children, _currentComment.depth);
                    foreach (var comment in list)
                    {
                        parentPanel.Children.Add(new CommentControl(comment));
                        //this.InsertMoreComment(comment);
                    }
                }
            }
            parentPanel?.Children.Remove(this);
        }

        private async void UpvoteButton_Click(object sender, RoutedEventArgs e)
        {
            await CommentLikedAsync();
            await _repository.UpdateAsync(new UserPreference
            {
                Username = UserFactory.GetInfo().name,
                SubredditName = _currentComment.subreddit,
                PriorityMultiplier = 1
            });
        }

        private async void DownvoteButton_Click(object sender, RoutedEventArgs e)
        {
            await CommentDislikedAsync();
            await _repository.UpdateAsync(new UserPreference
            {
                Username = UserFactory.GetInfo().name,
                SubredditName = _currentComment.subreddit,
                PriorityMultiplier = 1
            });
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
            await _redditApiConsumer.VoteAsync(_currentComment, newDirection);
        }

        public async Task CommentDislikedAsync()
        {
            var newDirection = GetNewVoteDirection(-1);
            UpdateVoteUi(newDirection);
            await _redditApiConsumer.VoteAsync(_currentComment, newDirection);
        }

        //Grimt i know.. what to do? det er et midlertidligt workaround
        private readonly Style _upvoteClickedStyle = Application.Current.Resources["LikeButtonClicked"] as Style;

        private readonly Style _upvoteNotClickedStyle = Application.Current.Resources["LikeButton"] as Style;
        private readonly Style _downvoteClickedStyle = Application.Current.Resources["DislikeButtonClicked"] as Style;
        private readonly Style _downvoteNotClickedStyle = Application.Current.Resources["DislikeButton"] as Style;

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
            if (_insertCommentPanel == null)
            {
                CreateCommentPanel();
                TextInfoPanel.Children.Add(_insertCommentPanel);
            }
            else
            {
                TextInfoPanel.Children.Remove(_insertCommentPanel);
                _insertCommentPanel = null;
            }
        }

        private void CreateCommentPanel()
        {
            _insertCommentPanel = null;
            _insertCommentPanel = new RelativePanel() { Margin = new Thickness(0, 40, 0, 0) };
            _commentTextBox = new TextBox()
            {
                Height = 200,
                Width = 600,
                AcceptsReturn = true,
                TextWrapping = TextWrapping.Wrap,
                IsSpellCheckEnabled = true,
                Language = "en-US"
            };

            RelativePanel.SetBelow(new Button()
            {
                Content = "Save",
                Margin = new Thickness(0, 10, 10, 0)
            }, _commentTextBox);

            _errorText = new TextBlock()
            {
                Visibility = Visibility.Collapsed,
                Margin = new Thickness(10, 7, 0, 0),
                FontSize = 14
            };
            RelativePanel.SetRightOf(_errorText, new Button()
            {
                Content = "Save",
                Margin = new Thickness(0, 10, 10, 0)
            });
            RelativePanel.SetBelow(_errorText, _commentTextBox);
            RelativePanel.SetAlignVerticalCenterWith(_errorText, new Button()
            {
                Content = "Save",
                Margin = new Thickness(0, 10, 10, 0)
            });
            new Button()
            {
                Content = "Save",
                Margin = new Thickness(0, 10, 10, 0)
            }.Click += CommentSaveClick;

            _insertCommentPanel.Children.Add(_commentTextBox);
            _insertCommentPanel.Children.Add(new Button()
            {
                Content = "Save",
                Margin = new Thickness(0, 10, 10, 0)
            });
            _insertCommentPanel.Children.Add(_errorText);
        }

        private async void CommentSaveClick(object sender, RoutedEventArgs e)
        {
            await InsertComment(_currentComment);
        }

        private async Task InsertComment(AbstractCommentable abstractCommentableToCommentOn)
        {
            var text = _commentTextBox.Text;
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                _errorText.Text = "We need something in the textbox";
                _errorText.Visibility = Visibility.Visible;
            }
            else
            {
                var old = new DateTime(1970, 1, 1);
                var totaltime = DateTime.Now - old;
                var timeInSeconds = (int) totaltime.TotalSeconds;
                var newComment = new Comment()
                {
                    body = _commentTextBox.Text,
                    author = "ASD",
                    depth = _currentComment.depth + 1,
                    created_utc = timeInSeconds
                };

                await _redditApiConsumer.CreateCommentAsync(abstractCommentableToCommentOn, newComment.body);

                _currentComment.Replies.Insert(0, newComment);

                _commentTextBox.Text = "";
                TextInfoPanel.Children.Remove(_insertCommentPanel);
                _insertCommentPanel = null;

                CommentStackPanel.Children.Insert(1, new CommentControl(newComment));
                await _repository.UpdateAsync(new UserPreference
                {
                    Username = UserFactory.GetInfo().name,
                    SubredditName = _currentComment.subreddit,
                    PriorityMultiplier = 3
                });
            }
        }
    }
}