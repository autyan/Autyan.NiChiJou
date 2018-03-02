using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Autyan.NiChiJou.Core.Mvc.Extension
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UserHostSettings(this IWebHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("hosting.json", optional: true)
                .Build();

            builder.UseConfiguration(config);

            return builder;
        }
    }
}
