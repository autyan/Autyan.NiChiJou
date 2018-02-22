using System;
using Autyan.NiChiJou.Core.Config;
using Microsoft.Extensions.DependencyInjection;

namespace Autyan.NiChiJou.Core.Mvc.DistributedCache
{
    public static class DistributedCacheExtension
    {
        public static IServiceCollection AddIdentityCache(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            services.AddOptions();
            services.Configure(new Action<IdentityCacheOptions>(options =>
            {
                options.Configuration = ResourceConfiguration.RedisAddress;
                options.InstanceName = ResourceConfiguration.RedisInstanceName;
            }));
            services.Add(ServiceDescriptor.Singleton<IIdentityCache, IdentityCache>());
            return services;
        }
    }
}
