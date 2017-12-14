using Entities.RedditEntities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UITEST.CustomUI;
using UITEST.ViewModel;
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
using Gorilla.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace UITEST.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PostPage : Page
    {
        private readonly PostPageViewModel _vm;
        private RelativePanel CommentPanel;
        private TextBox CommentTextBox;
        private TextBlock errorText;
        
        public PostPage()
        {
            this.InitializeComponent();
            LoadingRing.IsActive = true;

            _vm = App.ServiceProvider.GetService<PostPageViewModel>();
            DataContext = _vm;
            SetEventMethods();
        }

        private void CommentsReadyEvent()
        {
            LoadingRing.IsActive = false;
            DrawComments();
        }

        private void SetEventMethods()
        {
            SizeChanged += ChangeListViewWhenSizedChanged;
            _vm.CommentsReadyEvent += CommentsReadyEvent;
            _vm.Like += LikeSuccesful;
            _vm.Dislike += DislikeSuccesful;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var post = e.Parameter as Post;
            _vm.Initialize(post);

            SetUpTimeText();
        }

        private void SetUpTimeText()
        {
            TimeText.Text = TimeHelper.CalcCreationDateByUser(_vm.CurrentPost);
        }

        private void ChangeListViewWhenSizedChanged(object sender, SizeChangedEventArgs e)
        {
            PostView.Height = e.NewSize.Height-commandBar.ActualHeight;
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
        
        private void CreateCommentPanel()
        {
            if (CommentPanel != null)
            {
                CommentPanel = null;
            }
            CommentPanel = new RelativePanel() { Margin = new Thickness(0, 40, 0, 0)};
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
            errorText = new TextBlock() { Visibility = Visibility.Collapsed, Margin = new Thickness(10, 7, 0, 0), FontSize = 14};
            RelativePanel.SetRightOf(errorText, SubmitButton);
            RelativePanel.SetBelow(errorText, CommentTextBox);
            RelativePanel.SetAlignVerticalCenterWith(errorText, SubmitButton);
            SubmitButton.Click += CommentSaveClick;
            
            CommentPanel.Children.Add(CommentTextBox);
            CommentPanel.Children.Add(SubmitButton);
            CommentPanel.Children.Add(errorText);
        }

        private void PostTextComment_Click(object sender, RoutedEventArgs e)
        {
            if (CommentPanel == null)
            {
                CreateCommentPanel();
                ExtraStuff.Children.Add(CommentPanel);
            }
            else
            {
                ExtraStuff.Children.Remove(CommentPanel);
                CommentPanel = null;
            }
        }

        private void CommentSaveClick(object sender, RoutedEventArgs e)
        {
            InsertComment(_vm.CurrentPost);
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
                var old = new DateTime(1970, 1, 1);
                var totaltime = DateTime.Now - old;
                int timeInSeconds = (int)totaltime.TotalSeconds;
                var newComment = new Comment()
                { 
                    body = CommentTextBox.Text,
                    author = "ASD",
                    created_utc = timeInSeconds
                
                
            };
                _vm.AddCommentAsync(abstractCommentableToCommentOn, newComment);
                PostView.Items.Insert(2, new CommentControl(newComment));
                ExtraStuff.Children.Remove(CommentPanel);
                CommentPanel = null;
            }
        }
        

        private void DrawComments()
        {
            foreach (var comment in _vm.CurrentPost.Replies)
            {
                if (comment.body == null) { continue; }
                var TopCommentPanel = new CommentControl(comment);
                PostView.Items.Add(TopCommentPanel);
            }
        }

        private void Upvote_Click(object sender, RoutedEventArgs e)
        {
            _vm.PostLikedAsync();
        }

        private void Downvote_Click(object sender, RoutedEventArgs e)
        {
            _vm.PostDislikedAsync();
        }



        //Grimt i know.. what to do? det er et midlertidligt workaround
        private Style UpvoteClickedStyle = App.Current.Resources["LikeButtonClicked"] as Style;
        private Style UpvoteNotClickedStyle = App.Current.Resources["LikeButton"] as Style;
        private Style DownvoteClickedStyle = App.Current.Resources["DislikeButtonClicked"] as Style;
        private Style DownvoteNotClickedStyle = App.Current.Resources["DislikeButton"] as Style;

        private void LikeSuccesful()
        {
            int votes;
            int.TryParse(Votes.Text, out votes);

            if (Upvote.Style.Equals(UpvoteClickedStyle))
            {
                Upvote.Style = UpvoteNotClickedStyle;
            }
            else
            {
                Upvote.Style = UpvoteClickedStyle;
            }
            Downvote.Style = DownvoteNotClickedStyle;
        }

        private void DislikeSuccesful()
        {
            int votes;
            int.TryParse(Votes.Text, out votes);

            if (Downvote.Style.Equals(DownvoteClickedStyle))
            {
                Downvote.Style = DownvoteNotClickedStyle;
            }
            else
            {
                Downvote.Style = DownvoteClickedStyle;
            }
            Upvote.Style = UpvoteNotClickedStyle;
        }
    }
}