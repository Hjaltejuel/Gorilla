using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UITEST.Model;

namespace UITEST.ViewModel
{
    public class PostPageViewModel : BaseViewModel
    {
        private Post currentpost;

        public Post CurrentPost
        {
            get { return currentpost; }
            set {
                currentpost = value;
                OnPropertyChanged("CurrentPost");
            }
        }

        public AbstractCommentable FocusedAbstractCommentable { get; set; }

        public PostPageViewModel()
        {
        }

        public void Initialize(Post post)
        {
            CurrentPost = post;

            //    Title = post.Title;
            //    Text = post.Text;
            //    Author = post.Author;
            //    NumOfComments = post.NumOfComments;
            //    NumOfVotes = post.NumOfVotes;
            //    Comments = post.Comments;
        }

        //private string title;

        //public string Title
        //{
        //    get { return title; }
        //    set
        //    {
        //        title = value;
        //        OnPropertyChanged("Title");
        //    }
        //}

        //private string text;

        //public string Text
        //{
        //    get { return text; }
        //    set
        //    {
        //        text = value;
        //        OnPropertyChanged("Text");
        //    }
        //}

        //private string author;

        //public string Author
        //{
        //    get { return author; }
        //    set { author = value; }
        //}

        //private int numOfComments;

        //public int NumOfComments
        //{
        //    get { return numOfComments; }
        //    set
        //    {
        //        numOfComments = value;
        //        OnPropertyChanged("NumOfComments");
        //    }
        //}

        //private int numOfVotes;

        //public int NumOfVotes
        //{
        //    get { return numOfVotes; }
        //    set
        //    {
        //        numOfVotes = value;
        //        OnPropertyChanged("NumOfVotes");
        //    }
        //}

        //private List<Comment> comments;

        //public List<Comment> Comments
        //{
        //    get { return comments; }
        //    set
        //    {
        //        comments = value;
        //        OnPropertyChanged("Comments");
        //    }
        //}
    }
}
