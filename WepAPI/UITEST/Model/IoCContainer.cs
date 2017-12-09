using Gorilla.AuthenticationGorillaAPI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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


            return services.BuildServiceProvider();
        }
    }
}
