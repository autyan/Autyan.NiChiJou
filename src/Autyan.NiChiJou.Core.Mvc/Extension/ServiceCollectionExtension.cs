using System;
using Autyan.NiChiJou.Core.Context;
using Autyan.NiChiJou.Core.Mvc.Authorization;
using Autyan.NiChiJou.Core.Mvc.Context;
using Autyan.NiChiJou.Core.Mvc.DistributedCache;
using Autyan.NiChiJou.Core.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Autyan.NiChiJou.Core.Mvc.Extension
{
    public static class ServiceCollectionExtension
    {
        public static AuthenticationBuilder AddCookieAuthentication(this AuthenticationBuilder builder, IConfiguration configuration)
        {
            builder.AddCookie(configuration["Cookie:Schema"], options =>
            {
                options.LoginPath = configuration["Cookie:LoginPath"];
                options.LogoutPath = configuration["Cookie:LogoutPath"];
                options.ExpireTimeSpan = TimeSpan.FromMinutes(double.Parse(configuration["Cookie:Expiration"]));
            });

            builder.Services.AddOptions()
            .Configure<AutyanCookieOptions>(configuration.GetSection("Cookie"));

            return builder;
        }

        public static AuthenticationBuilder AddServiceTokenAuthentication(this AuthenticationBuilder builder, IConfiguration configuration)
        {
            builder.AddServiceToken(configuration["ServiceToken:Schema"],
                options =>
                {
                    options.AuthenticationSchema = configuration["ServiceToken:Schema"];
                    options.RequestMaxAgeSeconds = ulong.Parse(configuration["ServiceToken:MaxAge"]);
                });
            return builder;
        }

        public static IServiceCollection AddMvcComponent(this IServiceCollection services)
        {
            services.AddTransient<SignInManager, SignInManager>();
            services.TryAdd(ServiceDescriptor.Transient(typeof(IIdentityContext<>), typeof(SessionIdentityContext<>)));
            return services;
        }

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