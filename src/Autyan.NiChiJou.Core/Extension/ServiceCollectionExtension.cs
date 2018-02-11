using System.IO;
using Autyan.NiChiJou.Core.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Autyan.NiChiJou.Core.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddResourceConfiguration(this IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
            ResourceConfiguration.SetConfigurationRoot(configuration);
            return services;
        }
    }
}
