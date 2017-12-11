using Entities.RedditEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UITEST.Model;
using Windows.UI.Xaml.Controls;

namespace UITEST.CustomUI
{
    public class CustomButton : Button
    {
        public AbstractCommentable Commentable { get; private set; }

        public CustomButton(AbstractCommentable _abstractCommentable)
        {
            Commentable = _abstractCommentable;
        }
    }
}