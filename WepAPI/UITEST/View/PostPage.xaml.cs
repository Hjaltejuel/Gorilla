using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UITEST.CustomUI;
using UITEST.Model;
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
        public PostPage()
        {
            this.InitializeComponent();
            _vm = new PostPageViewModel()
            {
                GoToHomePageCommand = new RelayCommand(o => Frame.Navigate(typeof(MainPage))),
                GoToDiscoverPageCommand = new RelayCommand(o => Frame.Navigate(typeof(DiscoverPage))),
                GoToProfilePageCommand = new RelayCommand(o => Frame.Navigate(typeof(ProfilePage))),
                GoToTrendingPageCommand = new RelayCommand(o => Frame.Navigate(typeof(TrendingPage)))
            };
            DataContext = _vm;
            SizeChanged += ChangeListViewWhenSizedChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var post = e.Parameter as Post;
            _vm.Initialize(post);

            if (_vm.CurrentPost.Comments.Count > 0)
            {
                CreateComments();
            }
        }

        private void ChangeListViewWhenSizedChanged(object sender, SizeChangedEventArgs e)
        {
            if (CommentField.Visibility == Visibility.Collapsed)
            {
                CommentsView.Height = e.NewSize.Height - (50 + commandBar.ActualHeight + TopInfo.ActualHeight + verticalSplitter.Height);
            }
            else
            {
                CommentsView.Height = e.NewSize.Height - (50 + commandBar.ActualHeight + TopInfo.ActualHeight + verticalSplitter.Height + CommentField.ActualHeight);
            }
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

        private void CommentText_Click(object sender, RoutedEventArgs e)
        {
            var CommentBtn = sender as CustomButton;
            if (_vm.FocusedAbstractCommentable == null)
                _vm.FocusedAbstractCommentable = CommentBtn.AbstractCommentable;

            if (CommentField.Visibility == Visibility.Collapsed)
            {
                CommentField.Visibility = Visibility.Visible;
                CommentsView.Height -= CommentField.ActualHeight;
            }
            else if (CommentField.Visibility == Visibility.Visible)
            {
                if (_vm.FocusedAbstractCommentable.Equals(CommentBtn.AbstractCommentable))
                { 
                    CommentsView.Height += CommentField.ActualHeight;
                    CommentField.Visibility = Visibility.Collapsed;
                }
                else
                {
                    _vm.FocusedAbstractCommentable = CommentBtn.AbstractCommentable;
                }
            }
        }

        private void CommentSaveClick(object sender, RoutedEventArgs e)
        {
            InsertComment(_vm.FocusedAbstractCommentable);
        }

        private void InsertComment(AbstractCommentable abstractCommentableToCommentOn)
        {
            if (!CommentTextBox.Text.Equals(""))
            {
                abstractCommentableToCommentOn.InsertComment(new Comment()
                {
                    Text = CommentTextBox.Text,
                    Author = "Unidentified User"
                });
                CommentTextBox.Text = "";
                //Refresh comments
                CommentsView.Items.Clear();
                CreateComments();
            }
        }
        private StackPanel CreateCommentPanel(Comment comment)
        {
            var SingleCommentStackPanel = new StackPanel();
            var SingleCommentPanel = new RelativePanel()
            {
                Padding = new Thickness(0, 0, 0, 0)
            };

            RelativePanel TextPanel = SetupCommentTextPanel(comment);
            RelativePanel OpOgNedPanel = SetUpLikeAndDislikePanel(comment);
            RelativePanel TextExtraPanel = SetUpExtraCommentInfo(comment);
            SingleCommentPanel.Children.Add(OpOgNedPanel);
            SingleCommentPanel.Children.Add(TextPanel);
            SingleCommentPanel.Children.Add(TextExtraPanel);

            RelativePanel.SetRightOf(TextPanel, OpOgNedPanel);
            RelativePanel.SetBelow(TextExtraPanel, TextPanel);

            SingleCommentStackPanel.Children.Add(SingleCommentPanel);
            return SingleCommentStackPanel;
        }
        private void CreateComments()
        {
            foreach (var comment in _vm.CurrentPost.Comments)
            {
                //Tegne Posten i gui
                var CommentPanel = new StackPanel
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = new SolidColorBrush(Colors.Gray),
                    Margin = new Thickness(0, 0, 0, 10)
                };

                var SingleCommentPanel = CreateCommentPanel(comment);

                CommentPanel.Children.Add(SingleCommentPanel);
                CommentsView.Items.Add(CommentPanel);

                // Indsætte subcomments rekusivt
                if (comment.Comments.Count > 0)
                {
                    FillSubComments(comment.Comments, 1, SingleCommentPanel);
                }
            }
        }
        //Panel parent, ObservableCollection<Comment> subComments, int depth
        private void FillSubComments(ObservableCollection<Comment> subComments, int depth, StackPanel parentPanel)
        {
            foreach (var subComment in subComments)
            {
                var SingleCommentPanel = CreateCommentPanel(subComment);
                var newIndentation = parentPanel.Margin.Left + 30;
                SingleCommentPanel.Margin = new Thickness(newIndentation, 0, 0, 0);

                parentPanel.Children.Add(SingleCommentPanel);

                if (subComment.Comments.Count > 0)
                {
                    FillSubComments(subComment.Comments, ++depth, SingleCommentPanel);
                }
            }
        }

        private RelativePanel SetUpLikeAndDislikePanel(Comment comment)
        {
            var OpOgNedPanel = new RelativePanel() { Margin = new Thickness(10, 10, 15, 0) };
            var style = App.Current.Resources["LikeDisLikeButtonStyle"] as Style;
            var LikeButton = new CustomButton(comment) {Content = "Like", Style = App.Current.Resources["LikeDisLikeButtonStyle"] as Style, Width = 20 , Height = 20};
            LikeButton.Click += VoteButton_Clicked;
            var DisLikeButton = new CustomButton(comment) { Content = "Dislike", Style = App.Current.Resources["LikeDisLikeButtonStyle"] as Style, Width = 20, Height = 20 };
            DisLikeButton.Click += VoteButton_Clicked;
            RelativePanel.SetBelow(DisLikeButton, LikeButton); ;
            OpOgNedPanel.Children.Add(LikeButton);
            OpOgNedPanel.Children.Add(DisLikeButton);
            return OpOgNedPanel;
        }

        private RelativePanel SetUpExtraCommentInfo(Comment comment)
        {
            var TextInfoPanel = new RelativePanel() { Margin = new Thickness(20, 0, 0, 0) };
            var CommentButton = new CustomButton(comment) { Content = "comment",  };
            CommentButton.Click += CommentText_Click;
            TextInfoPanel.Children.Add(CommentButton);
            return TextInfoPanel;
        }

        private RelativePanel SetupCommentTextPanel(Comment comment)
        {
            var TextPanel = new RelativePanel() { Margin = new Thickness(0, 0, 0, 0) };
            var AuthorTextBlock = new TextBlock() { Text = comment.Author, FontSize = 10, Margin = new Thickness(0, 0, 0, 0) };
            var PointsTextBlock = new TextBlock() { FontSize = 10, Margin = new Thickness(7, 0, 0, 0) };

            Binding binding = new Binding();
            binding.Path = new PropertyPath("NumOfVotes");
            binding.Source = comment; 
            BindingOperations.SetBinding(PointsTextBlock, TextBlock.TextProperty, binding);

            var CommentTextBlock = new TextBlock() { Text = comment.Text, Margin = new Thickness(0, 10, 10, 10) };
            RelativePanel.SetBelow(CommentTextBlock, AuthorTextBlock);
            RelativePanel.SetRightOf(PointsTextBlock, AuthorTextBlock);
            TextPanel.Children.Add(AuthorTextBlock);
            TextPanel.Children.Add(CommentTextBlock);
            TextPanel.Children.Add(PointsTextBlock);
            return TextPanel;
        }

        private void VoteButton_Clicked(object sender, RoutedEventArgs e)
        {
            var btn = sender as CustomButton;
            if (btn.Content.Equals("Like"))
            {
                btn.AbstractCommentable.NumOfVotes += 1;
            }
            else if (btn.Content.Equals("Dislike"))
            {
                btn.AbstractCommentable.NumOfVotes -= 1;
            }
        }

        private void PostVoteButton_Clicked(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn.Content.Equals("Like"))
            {
                _vm.CurrentPost.NumOfVotes += 1;
            }
            else if (btn.Content.Equals("Dislike"))
            {
                _vm.CurrentPost.NumOfVotes -= 1;
            }
        }
    }
}
