using Entities.RedditEntities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UITEST.ViewModel;

namespace UITEST.Model
{
    public class AbstractCommentable : INotifyPropertyChanged
    {
        public AbstractCommentable()
        {
            Comments = new ObservableCollection<Comment>();
        }

        public string Author { get; set; }

        private int numOfVotes;

        public int NumOfVotes
        {
            get { return numOfVotes; }
            set
            {
                numOfVotes = value;
                OnPropertyChanged("NumOfVotes");
            }
        }


        private string text;

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }

        public ObservableCollection<Comment> Comments { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void InsertComment(Comment Comment)
        {
            Comments.Insert(0, Comment);
            OnPropertyChanged("Comments");
        }
    }
}
