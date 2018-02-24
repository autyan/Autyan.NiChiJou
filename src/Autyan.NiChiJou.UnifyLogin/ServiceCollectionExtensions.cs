using Microsoft.Extensions.DependencyInjection;

namespace Autyan.NiChiJou.UnifyLogin
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUnifyLogin(this IServiceCollection services)
        {
            services.AddSingleton<LoginApiManager, LoginApiManager>();
            return services;
        }
    }
}
