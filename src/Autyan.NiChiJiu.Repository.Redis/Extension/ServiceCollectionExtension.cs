using Autyan.NiChiJiu.Repository.Redis.Identity;
using Autyan.NiChiJou.Repository.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Autyan.NiChiJiu.Repository.Redis.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRedis(this IServiceCollection services)
        {
            services.AddScoped<ISessionDataRepository, SessionDataRepository>();
            return services;
        }
    }
}
