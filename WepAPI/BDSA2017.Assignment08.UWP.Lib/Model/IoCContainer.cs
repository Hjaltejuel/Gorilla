﻿using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using UI.Lib.Authentication.GorillaAuthentication;
using UI.Lib.Model.GorillaRepositories;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRepositories;
using UI.Lib.Model.RedditRestInterfaces;
using UI.Lib.ViewModel;
using UI.Lib.Authentication;
using UI.Lib.ViewModel;
using BDSA2017.Assignment08.UWP.ViewModel;
using BDSA2017.Assignment08.UWP.Model.GorillaRestInterfaces;
using BDSA2017.Assignment08.UWP.Model.GorillaRepositories;

namespace UI.Lib.Model
{
    public class IoCContainer
    {
        public static IServiceProvider Create() => ConfigureServices();

        private static IServiceProvider ConfigureServices()
        {

            IServiceCollection services = new ServiceCollection();
            
            services.AddScoped<ISettings, Settings>();
            services.AddScoped<DelegatingHandler, AuthorizedHandler>();
            services.AddScoped<IAuthenticationHelper, AuthenticationHelper>();
            services.AddScoped<INavigationService, NavigationService>();
            services.AddScoped<IRestSubredditRepository, RestSubredditRepository>();
            services.AddScoped<IRestSubredditConnectionRepository, RestSubredditConnectionRepository>();
            services.AddScoped<IRestUserPreferenceRepository, RestUserPreferenceRepository>();
            services.AddScoped<IRestUserRepository, RestUserRepository>();
            services.AddScoped<IRestPostRepository, RestPostRepository>();
            services.AddScoped<IRedditApiConsumer, RedditConsumerController>();
            services.AddScoped<IRedditAuthHandler, RedditAuthHandler>();
            services.AddScoped<IRestCategoryRepository, RestCategoryRepository>();
            services.AddScoped<IUserHandler, UserHandler>();

            services.AddScoped<MainPageViewModel>();
            services.AddScoped<ThankYouForChoosingViewModel>();
            services.AddScoped<ChooseYourCategoriesViewModel>();
            services.AddScoped<DiscoverPageViewModel>();
            services.AddScoped<PostPageViewModel>();
            services.AddScoped<ProfilePageViewModel>();
            services.AddScoped<CreatePostPageViewModel>();
            services.AddScoped<LoginPageViewModel>();
            services.AddScoped<SubredditPageViewModel>();
            services.AddScoped<CommentViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
