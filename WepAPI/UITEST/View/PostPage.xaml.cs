﻿using Entities.RedditEntities;
using Microsoft.Extensions.DependencyInjection;
using UI.Lib.ViewModel;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Castle.Core.Internal;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using System;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Border = Windows.UI.Xaml.Controls.Border;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace UITEST.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PostPage : Page
    {
        private readonly PostPageViewModel _vm;
        
        public PostPage()
        {
            InitializeComponent();
            LoadingRing.IsActive = true;
            _vm = App.ServiceProvider.GetService<PostPageViewModel>();
            DataContext = _vm;
            SetEventMethods();
        }
        private void SetNumberOfCommentsTextInSingularOrPlural()
        {
            comments.Text = _vm.CurrentPost.Replies.Count == 1 ? "comment" : "comments";
        }

        private void CommentsReadyEvent()
        {
            LoadingRing.IsActive = false;
            DrawRootComments();
            SetNumberOfCommentsTextInSingularOrPlural();

        }
        private void SetEventMethods()
        {
            SizeChanged += ChangeListViewWhenSizedChanged;
            _vm.CommentsReadyEvent += CommentsReadyEvent;
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var post = e.Parameter as Post;
            if (post == null || post.selftext.IsNullOrEmpty())
            {
                TextPanel.Visibility = Visibility.Collapsed;
            }
            await _vm.Initialize(post);
        }
        private void ChangeListViewWhenSizedChanged(object sender, SizeChangedEventArgs e)
        {
            PostView.Height = e.NewSize.Height-commandBar.ActualHeight;
        }
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
        private void PostTextComment_Click(object sender, RoutedEventArgs e)
        {
            CommentTextBox.Text = "";
            ErrorText.Visibility = Visibility.Collapsed;
            CommentPanel.Visibility = CommentPanel.Visibility.Equals(Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }

        private async void CommentSaveClick(object sender, RoutedEventArgs e)
        {
            var text = CommentTextBox.Text;
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                ErrorText.Visibility = Visibility.Visible;
            }
            else
            {
                CommentPanel.Visibility = Visibility.Collapsed;
                var newComment = await _vm.CreateComment(_vm.CurrentPost, text);
                var topCommentPanel = CreateBorderedCommentPanel(newComment);

                PostView.Items?.Insert(2, topCommentPanel);
            }
        }
        private Border CreateBorderedCommentPanel(Comment comment)
        {
            var topCommentPanel = new CommentControl(comment)
            {
                Margin = new Thickness(10, 0, 0, 10)
            };
            var border = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(0, 10, 0, 0),
                Child = topCommentPanel
            };
            return border;
        }
        private void DrawRootComments()
        {
            foreach (var _comment in _vm.CurrentPost.Replies)
            {
                var comment = _comment as Comment;
                if (comment?.body == null) { continue; }
                var topCommentPanel = CreateBorderedCommentPanel(comment);
                //Make root comment bordered

                PostView.Items.Add(topCommentPanel);
            }
        }
    }
}