using Entities.RedditEntities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        public PostPage()
        {
            this.InitializeComponent();
            LoadingRing.IsActive = true;
            _vm = new PostPageViewModel()
            {
                GoToHomePageCommand = new RelayCommand(o => Frame.Navigate(typeof(MainPage))),
                GoToDiscoverPageCommand = new RelayCommand(o => Frame.Navigate(typeof(DiscoverPage))),
                GoToProfilePageCommand = new RelayCommand(o => Frame.Navigate(typeof(ProfilePage))),
                GoToTrendingPageCommand = new RelayCommand(o => Frame.Navigate(typeof(TrendingPage)))
            };
            DataContext = _vm;
            SizeChanged += ChangeListViewWhenSizedChanged;
            _vm.CommentsReadyEvent += _vm_CommentsReadyEvent;
        }

        private void _vm_CommentsReadyEvent()
        {
            LoadingRing.IsActive = false;
            DrawComments();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var post = e.Parameter as Post;
            _vm.Initialize(post);
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
            SubmitButton.Click += CommentSaveClick;
            
            CommentPanel.Children.Add(CommentTextBox);
            CommentPanel.Children.Add(SubmitButton);
        }

        private void PostTextComment_Click(object sender, RoutedEventArgs e)
        {
            var CommentBtn = sender as Button;

            if (CommentPanel == null)
            {
                CreateCommentPanel();
                var CommentExtraTextPanel = CommentBtn.Parent as RelativePanel;
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
            if (!CommentTextBox.Text.Equals(""))
            {
                var newComment = new Comment()
                {
                    body = CommentTextBox.Text,
                    author = "ASD"
                };

                _vm.AddComment(abstractCommentableToCommentOn, newComment);

                CommentTextBox.Text = "";
                ExtraStuff.Children.Remove(CommentPanel);
                CommentPanel = null;

                PostView.Items.Insert(2, new CommentControl(newComment));
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
            _vm.CurrentPost.score += 1;
        }

        private void Downvote_Click(object sender, RoutedEventArgs e)
        {
            _vm.CurrentPost.score -= 1;
        }
    }
}
