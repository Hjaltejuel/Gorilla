using System;
using Microsoft.Extensions.DependencyInjection;
using BDSA2017.Assignment08.UWP.ViewModels;
using System.IO;
using Windows.Storage;
using Microsoft.Data.Sqlite;

namespace BDSA2017.Assignment08.UWP.Models
{
    public class IoCContainer
    {
        public static IServiceProvider Create() => ConfigureServices();
       
        
        private static IServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            return services.BuildServiceProvider();
        }
    }
}
