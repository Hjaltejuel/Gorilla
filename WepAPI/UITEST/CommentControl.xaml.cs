﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UITEST.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public CommentControl(Comment comment)
        {
            this.InitializeComponent();
            currentComment = comment;
        }

        private void UpvoteButton_Click(object sender, RoutedEventArgs e)
        {
            currentComment.NumOfVotes++;
        }

        private void DownvoteButton_Click(object sender, RoutedEventArgs e)
        {
            currentComment.NumOfVotes--;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
