using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Autyan.NiChiJou.Core.Mvc.Extension
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UserAppsettings(this IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((builderContext, config) =>
            {
                var env = builderContext.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            });
            return builder;
        }
    }
}
