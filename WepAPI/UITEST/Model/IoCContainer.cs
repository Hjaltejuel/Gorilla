using Entities.RedditEntities;
using Gorilla.AuthenticationGorillaAPI;
using Microsoft.Extensions.DependencyInjection;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UITEST.ViewModel;
using WebApplication2.Models.GorillaApiConsumeRepositories;

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
            services.AddScoped<ISubredditRepository, RestSubredditRepository>();
            services.AddScoped<ISubredditConnectionRepository, RestSubredditConnectionRepository>();
            services.AddScoped<IUserPreferenceRepository, RestUserPreferenceRepository>();
            services.AddScoped<IRedditAPIConsumer, RedditConsumerController>();
            services.AddScoped<MainPageViewModel>();
            services.AddScoped<DiscoverPageViewModel>();
            services.AddScoped<PostPageViewModel>();
            services.AddScoped<ProfilePageViewModel>();
            services.AddScoped<TrendingPageViewModel>();
            services.AddScoped<CreatePostPageViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
