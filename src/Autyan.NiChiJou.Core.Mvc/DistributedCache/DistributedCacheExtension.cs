using System;
using Microsoft.Extensions.DependencyInjection;

namespace Autyan.NiChiJou.Core.Mvc.DistributedCache
{
    public static class DistributedCacheExtension
    {
        public static IServiceCollection AddIdentityCache(this IServiceCollection services, Action<IdentityCacheOptions> setupAction)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));
            services.AddOptions();
            services.Configure(setupAction);
            services.Add(ServiceDescriptor.Singleton<IIdentityCache, IdentityCache>());
            return services;
        }
    }
}
