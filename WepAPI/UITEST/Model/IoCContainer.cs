using Gorilla.AuthenticationGorillaAPI;
using Microsoft.Extensions.DependencyInjection;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            services.AddScoped<ISubredditRepository, RestSubredditRepository>();
            services.AddScoped<IUserRepository, RestUserRepository>();
            services.AddScoped<IUserPreferenceRepository, RestUserPreferenceRepository>();
            services.AddScoped<ISubredditConnectionRepository, RestSubredditConnectionRepository>();



            return services.BuildServiceProvider();
        }
    }
}
