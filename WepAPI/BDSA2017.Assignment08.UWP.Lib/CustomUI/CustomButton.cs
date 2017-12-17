using Entities.RedditEntities;
using Windows.UI.Xaml.Controls;

namespace UI.Lib.CustomUI
{
    public class CustomButton : Button
    {
        public AbstractCommentable Commentable { get; }

        public CustomButton(AbstractCommentable abstractCommentable)
        {
            Commentable = abstractCommentable;
        }
    }
}