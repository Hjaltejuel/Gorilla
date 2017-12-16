using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Entities.RedditEntities
{
    public abstract class AbstractCommentable : INotifyPropertyChanged
    {
        public ObservableCollection<AbstractCommentable> Replies { get; set; }
        public int created_utc { get; set; }
        public string author { get; set; }
        //public int score { get; set; }
        private int Score;

        public int score
        {
            get => Score;
            set {
                if (value != Score)
                {
                    Score = value;
                    OnPropertyChanged("Score");
                }
            }
        }

        public string likes { get; set; }
        public bool can_mod_post { get; set; }
        public string name { get; set; }
        public int downs { get; set; }
        public int ups { get; set; }
        public int created { get; set; }
        public bool archived { get; set; }
        public bool edited { get; set; }
        public bool saved { get; set; }
        public string report_reasons { get; set; }
        public string approved_at_utc { get; set; }
        public string banned_by { get; set; }
        public string subreddit { get; set; }
        public string[] user_reports { get; set; }


        public void BuildReplies(Listing listing)
        {
            if (listing == null) { return; }
            foreach (var ch in listing.data.children)
            {
                Replies.Add(ch.data.ToObject<Comment>());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}