using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UITEST.Model;

namespace UITEST.Model
{
    public class Post : AbstractCommentable
    {
        private string title;

        public string Title
        {
            get { return title; }
            set {
                title = value;
                OnPropertyChanged("Title");
            }
        }
    }
}
