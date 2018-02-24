using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Autyan.NiChiJou.Core.Mvc.DistributedCache
{
    public static class DistributedCacheExtension
    {
        public static IServiceCollection AddIdentityCache(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            services.AddOptions();
            services.Configure(new Action<IdentityCacheOptions>(options =>
            {
                options.Configuration = configuration["IdentityCache:Server"];
                options.InstanceName = configuration["IdentityCache:Instance"];
            }));
            services.Add(ServiceDescriptor.Singleton<IIdentityCache, IdentityCache>());
            return services;
        }
    }
}
