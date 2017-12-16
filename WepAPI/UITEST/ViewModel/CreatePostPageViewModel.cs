using Entities.RedditEntities;
using Gorilla.AuthenticationGorillaAPI;
using Gorilla.Model;
using Model;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using UITEST.RedditInterfaces;
using Windows.UI.Popups;

namespace UITEST.ViewModel
{
    public class CreatePostPageViewModel : BaseViewModel
    {
        private readonly IRedditAPIConsumer _consumer;
        private Subreddit CurrentSubreddit;

        public delegate void PostSent();
        public event PostSent SentSuccesfulEvent;
        public event PostSent SentUnsuccesfulEvent;

        ICommand SubmitPostCommand;
        private string _title;
        public string title{ get { return _title; } set { _title = value; OnPropertyChanged(); }}
        private string _body;
        public string body { get { return _body; } set { _body = value; OnPropertyChanged(); } }     


        public Subreddit currentSubreddit
        {
            get { return CurrentSubreddit; }
            set
            {
                if (value != CurrentSubreddit)
                {
                    CurrentSubreddit = value;
                    OnPropertyChanged("CurrentSubreddit");
                }
            }
        }
        private readonly IRestUserPreferenceRepository _repository;

        public CreatePostPageViewModel(IAuthenticationHelper helper, INavigationService service, IRedditAPIConsumer consumer, IRestUserPreferenceRepository repository) : base(service)
        {
            _repository = repository;
            _consumer = consumer;
            _helper = helper;

            SubmitPostCommand = new RelayCommand(async o => { await CreateNewPostAsync(title, body); });
        }

        public async Task CreateNewPostAsync(string title, string body = "")
        {
            //if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title))
            //{
                var messageDialog = new MessageDialog("A post needs a title");
                messageDialog.ShowAsync();
                return;
            //}
            //var response = await _consumer.CreatePostAsync(CurrentSubreddit, title, "self", body);
            //if (response.Item1 == HttpStatusCode.OK)
            //{
            //    SentSuccesfulEvent.Invoke();
            //    await _repository.UpdateAsync(new Entities.UserPreference { Username = UserFactory.GetInfo().name, SubredditName = CurrentSubreddit.display_name, PriorityMultiplier = 5 });
            //}
            //else
            //{
            //    SentUnsuccesfulEvent.Invoke();
            //}
            
        }
    }
}
