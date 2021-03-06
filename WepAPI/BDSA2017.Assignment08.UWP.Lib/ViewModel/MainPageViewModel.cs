﻿using System.Threading.Tasks;
using Entities.GorillaEntities;
using UI.Lib.Authentication.GorillaAuthentication;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;
using System.Collections.Generic;
using System.Net;

namespace UI.Lib.ViewModel
{
    public class MainPageViewModel : SearchableViewModel
    {
        private readonly IUserHandler _userHandler;
        private readonly IRestSubredditRepository _restSubredditRepository;
        public delegate void MainReady();
        public event MainReady MainReadyEvent;
        public MainPageViewModel(IAuthenticationHelper helper, INavigationService service, IRedditApiConsumer consumer, IRestUserRepository repository, IUserHandler userHandler, IRestSubredditRepository restSubredditRepository) : base(service, consumer, restSubredditRepository)
        {
            _restSubredditRepository = restSubredditRepository;
            Repository = repository;
            _userHandler = userHandler;
            Initialize();
        }

        public async Task GeneratePosts()
        {
            var postsResult = await Consumer.GetHomePageContent();
            if (postsResult.Item1 == HttpStatusCode.OK)
            {
                Posts = (postsResult).Item2;
                MainReadyEvent.Invoke();
            }
        }
        public async Task Initialize()
        {
            await GeneratePosts();
        }
    }
}
