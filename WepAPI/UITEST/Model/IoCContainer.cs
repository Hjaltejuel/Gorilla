using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using UITEST.Authentication.GorillaAuthentication;
using UITEST.Model.GorillaRepositories;
using UITEST.Model.GorillaRestInterfaces;
using UITEST.Model.RedditRepositories;
using UITEST.Model.RedditRestInterfaces;
using UITEST.ViewModel;

namespace UITEST.Model
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
            services.AddScoped<IRedditApiConsumer, RedditConsumerController>();
            services.AddScoped<MainPageViewModel>();
            
           
            services.AddScoped<DiscoverPageViewModel>();
            services.AddScoped<PostPageViewModel>();
            services.AddScoped<ProfilePageViewModel>();
            services.AddScoped<TrendingPageViewModel>();
            services.AddScoped<CreatePostPageViewModel>();
            services.AddScoped<LoginPageViewModel>();
            services.AddScoped<SubredditPageViewModel>();
            services.AddScoped<CommentViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
