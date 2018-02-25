using Autyan.NiChiJou.Core.Mvc.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Autyan.NiChiJou.UnifyLogin
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUnifyLogin(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<LoginApiManager, LoginApiManager>()
                .AddAuthentication(options => options.DefaultScheme = configuration["Cookie:Schema"])
                .AddCookieAuthentication(configuration);
            return services;
        }
    }
}
