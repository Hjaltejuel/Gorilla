using Entities.RedditEntities;
using Gorilla.AuthenticationGorillaAPI;
using Gorilla.Model;
using Model;
using System.Diagnostics;
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

        public delegate void LoadingRingSwitch();
        public event LoadingRingSwitch LoadingRingOnOf;
        public ICommand SubmitPostCommand { get; set; }
        private string _title;
        public string title { get { return _title; } set { _title = value; OnPropertyChanged(); } }

        private string _body;
        public string body{ get { return _body; }set { _body = value; OnPropertyChanged(); }}
      
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
            MessageDialog messageDialog;
            if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title))
            {
                messageDialog = new MessageDialog("A post needs a title");
                messageDialog.ShowAsync();
                return;
            }
            LoadingRingOnOf.Invoke();
            var response = await _consumer.CreatePostAsync(CurrentSubreddit, title, "self", body);
            LoadingRingOnOf.Invoke();
            if (response.Item1 == HttpStatusCode.OK)
            {
                messageDialog = new MessageDialog("Back to subreddit", "Succes");
                messageDialog.Commands.Add(new UICommand("Ok", new UICommandInvokedHandler(BackToSubreddit)));
                await _repository.UpdateAsync(new Entities.UserPreference { Username = UserFactory.GetInfo().name, SubredditName = CurrentSubreddit.display_name, PriorityMultiplier = 5 });
            }
            else
            {
                messageDialog = new MessageDialog("Clicking Ok will keep you at the current page", "Could not create post");
            }
            messageDialog.ShowAsync();
        }

        private void BackToSubreddit(IUICommand command)
        {
            _service.GoBack();
        }
    }
}
