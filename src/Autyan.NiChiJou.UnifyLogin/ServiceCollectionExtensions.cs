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
                .AddSingleton<LoginAction, LoginAction>()
                .AddAuthentication(options => options.DefaultScheme = configuration["Cookie:Schema"])
                .AddCookieAuthentication(configuration);

            services.AddOptions();
            services.Configure<UnifyLoginOptions>(configuration.GetSection("Cookie"));
            return services;
        }
    }
}
