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
        private RelativePanel CommentPanel;
        private TextBox CommentTextBox;
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

        /*
         * <RelativePanel Grid.Row="3" Margin="10,0, 0, 10" Visibility="Collapsed" Name="CommentField">
                <TextBox Name="CommentTextBox" Height="200" Width="600" AcceptsReturn="True" TextWrapping="Wrap" IsSpellCheckEnabled="True" Language="en-US"/>
                <Button Content="Save" Click="CommentSaveClick"  RelativePanel.Below="  " Margin="0, 10, 10, 0"/>
            </RelativePanel>
         */
        private void CreateCommentPanel()
        {
            if (CommentPanel != null)
            {
                CommentPanel = null;
            }
            CommentPanel = new RelativePanel();

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
            SubmitButton.Click += CommentSaveClick;
            
            CommentPanel.Children.Add(CommentTextBox);
            CommentPanel.Children.Add(SubmitButton);
        }
        private void CommentText_Click(object sender, RoutedEventArgs e)
        {
            var CommentBtn = sender as CustomButton;

            CreateCommentPanel();

            _vm.FocusedAbstractCommentable = CommentBtn.AbstractCommentable;
            var CommentExtraTextPanel = CommentBtn.Parent as RelativePanel;
            var CommentPanel = CommentExtraTextPanel.Parent as RelativePanel;
            var CommentStackPanel = CommentPanel.Parent as StackPanel;

            CommentStackPanel.Children.Add(CommentPanel);
        }

        private void CommentSaveClick(object sender, RoutedEventArgs e)
        {
            InsertComment(_vm.FocusedAbstractCommentable);

            //if (CommentPanel.Visibility == Visibility.Visible)
            //{
            //    CommentPanel.Visibility = Visibility.Collapsed;
            //}
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
                CommentsView.Children.Clear();
                CreateComments();
            }
        }
        private StackPanel CreateCommentPanel(Comment comment, bool ColorBoolean)
        {
            var SingleCommentStackPanel = new StackPanel()
            {
                Margin = new Thickness(30, 0, 0, 30)
            };
            var SingleCommentPanel = new CommentControl(comment);
            
            if (ColorBoolean)
                SingleCommentStackPanel.Background = new SolidColorBrush(Color.FromArgb(255, 220, 220, 220));
            else
                SingleCommentStackPanel.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

            SingleCommentStackPanel.Children.Add(SingleCommentPanel);

            return SingleCommentStackPanel;
        }
        private void CreateComments()
        {
            foreach (var comment in _vm.CurrentPost.Comments)
            {
                //Tegne Posten i gui
                var RootCommentPanel = new StackPanel
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = new SolidColorBrush(Colors.Gray),
                    Margin = new Thickness(0, 0, 0, 10)
                };
                

                var TopCommentPanel = CreateCommentPanel(comment, false);
                TopCommentPanel.Margin = new Thickness(0, 0, 0, 0); //Reset margin for first comment
                RootCommentPanel.Children.Add(TopCommentPanel);
                CommentsView.Children.Add(RootCommentPanel);
                // Indsætte subcomments rekusivt
                if (comment.Comments.Count > 0)
                {
                    FillSubComments(comment.Comments, TopCommentPanel, true);
                }
            }
        }
        //Panel parent, ObservableCollection<Comment> subComments, int depth
        private void FillSubComments(ObservableCollection<Comment> subComments, StackPanel parentPanel, bool ColorBoolean)
        {
            foreach (var subComment in subComments)
            {
                var SubCommentPanel = CreateCommentPanel(subComment, ColorBoolean);

                parentPanel.Children.Add(SubCommentPanel);

                if (subComment.Comments.Count > 0)
                {
                    FillSubComments(subComment.Comments, SubCommentPanel, !ColorBoolean);
                }
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
