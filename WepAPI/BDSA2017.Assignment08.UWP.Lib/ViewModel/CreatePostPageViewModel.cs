using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Entities.GorillaEntities;
using UI.Lib.Authentication.GorillaAuthentication;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;
using Subreddit = Entities.RedditEntities.Subreddit;

namespace UI.Lib.ViewModel
{
    public class CreatePostPageViewModel : BaseViewModel
    {
        private readonly IRedditApiConsumer _consumer;
        private readonly IUserHandler _userHandler;
        private Subreddit _currentSubreddit;

        public delegate void LoadingRingSwitch();
        public event LoadingRingSwitch LoadingRingOnOf;
        public ICommand SubmitPostCommand { get; set; }
        private string _title;
        public string Title { get => _title;
            set { _title = value; OnPropertyChanged(); } }

        private string _body;
        public string Body{ get => _body;
            set { _body = value; OnPropertyChanged(); }}
      
        public Subreddit CurrentSubreddit
        {
            get => _currentSubreddit;
            set
            {
                if (value != _currentSubreddit)
                {
                    _currentSubreddit = value;
                    OnPropertyChanged();
                }
            }
        }
        private readonly IRestUserPreferenceRepository _repository;

        public CreatePostPageViewModel( INavigationService service, IRedditApiConsumer consumer, IRestUserPreferenceRepository repository, IUserHandler userHandler) : base(service)
        {
            _repository = repository;
            _consumer = consumer;
            _userHandler = userHandler;
          
            SubmitPostCommand = new RelayCommand(async o => { await CreateNewPostAsync(Title, Body); });
        }

        public async Task CreateNewPostAsync(string title, string body = "")
        {
            MessageDialog messageDialog = null;
            if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title))
            {
                messageDialog = new MessageDialog("A post needs a title");
                messageDialog.ShowAsync();
                return;
            }
            if (LoadingRingOnOf != null)
            {
                LoadingRingOnOf.Invoke();
                var response = await _consumer.CreatePostAsync(_currentSubreddit, title, "self", body);
                LoadingRingOnOf.Invoke();
                if (response.Item1 == HttpStatusCode.OK)
                {
                    messageDialog = new MessageDialog("Back to subreddit", "Succes");
                    messageDialog.Commands.Add(new UICommand("Ok", BackToSubreddit));
                    await _repository.UpdateAsync(new UserPreference
                    {
                        Username = _userHandler.GetUser().name,
                        SubredditName = _currentSubreddit.display_name,
                        PriorityMultiplier = 5
                    });
                }
                else
                {
                    messageDialog = new MessageDialog("Clicking Ok will keep you at the current page",
                        "Could not create post");
                }
            }
            messageDialog?.ShowAsync();
        }

        private void BackToSubreddit(IUICommand command)
        {
            Service.GoBack();
        }
    }
}
