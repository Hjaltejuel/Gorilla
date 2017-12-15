using Entities.RedditEntities;
using Gorilla.AuthenticationGorillaAPI;
using Gorilla.Model;
using Gorilla.View;
using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UITEST.View;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Collections.Generic;

namespace UITEST.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        public ICommand GoToCreatePostPageCommand { get; set; }
        bool firstTime = true;
        IRedditAPIConsumer _consumer;
        IRestUserRepository _repository;
        public ObservableCollection<Post> posts;
        public ObservableCollection<Post> Posts
        {
            get => posts;
            set
            {
                posts = value;
                OnPropertyChanged("Posts");
            }
        }

        public delegate void LoadingEvent();
        public event LoadingEvent PostsReadyEvent;
        public event LoadingEvent PostsStartedLoading;

        public MainPageViewModel(IAuthenticationHelper helper, INavigationService service, IRedditAPIConsumer consumer, IRestUserRepository repository) : base(service)
        {
            _consumer = consumer;
            _repository = repository;
            _helper = helper;
            Initialize();
        }

        public async Task GeneratePosts()
        {
            PostsStartedLoading.Invoke();
            await UserFactory.initialize(_consumer);
            await _repository.CreateAsync(new Entities.User { Username = UserFactory.GetInfo().name, PathToProfilePicture = "profilePicture.jpg" });
            Posts = await _consumer.GetHomePageContent();
            foreach (Post p in Posts)
            {
                if (p.is_self)
                {
                    p.thumbnail = "/Assets/Textpost.png";
                }
                else
                {
                    if (p.thumbnail == "default")
                    {
                        p.thumbnail = "/Assets/Externallink.png";
                    }
                }
            }
            PostsReadyEvent.Invoke();
        }
        public async Task Initialize()
        {
            if (await Authorize() != null)
            {
                await GeneratePosts();
            }
            else
            {
                if(firstTime == true)
                {
                    firstTime = false;
                }
                else
                {
                    await Initialize();
                }
            }
        }
    }
}
