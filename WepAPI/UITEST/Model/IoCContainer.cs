﻿using Entities.RedditEntities;
using Gorilla.AuthenticationGorillaAPI;
using Microsoft.Extensions.DependencyInjection;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Gorilla.ViewModel;
using UITEST.ViewModel;
using WebApplication2.Models.GorillaApiConsumeRepositories;
using Gorilla.Model.GorillaRestInterfaces;
using Gorilla.Model.GorillaRepositories;
using UITEST.Model;

namespace Gorilla.Model
{
    class IoCContainer
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
            services.AddScoped<IRedditAPIConsumer, RedditConsumerController>();
            services.AddScoped<MainPageViewModel>();
            services.AddScoped<User>();
            services.AddScoped<DiscoverPageViewModel>();
            services.AddScoped<PostPageViewModel>();
            services.AddScoped<ProfilePageViewModel>();
            services.AddScoped<TrendingPageViewModel>();
            services.AddScoped<CreatePostPageViewModel>();
            services.AddScoped<LoginPageViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
