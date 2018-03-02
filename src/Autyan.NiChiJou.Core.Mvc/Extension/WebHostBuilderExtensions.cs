using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Autyan.NiChiJou.Core.Mvc.Extension
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UserHostSettings(this IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((builderContext, config) =>
            {
                config.AddJsonFile("hosting.json", optional: true)
                    .AddEnvironmentVariables();
            });
            return builder;
        }
    }
}
