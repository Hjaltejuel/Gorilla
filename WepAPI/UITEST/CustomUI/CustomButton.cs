using Entities.RedditEntities;
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