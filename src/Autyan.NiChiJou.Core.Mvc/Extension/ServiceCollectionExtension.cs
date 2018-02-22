using System;
using Autyan.NiChiJou.Core.Config;
using Microsoft.Extensions.DependencyInjection;

namespace Autyan.NiChiJou.Core.Mvc.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddAutyanAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(ResourceConfiguration.AuthenticationScheme)
                .AddCookie(ResourceConfiguration.AuthenticationScheme, options =>
                {
                    options.LoginPath = ResourceConfiguration.LoginPath;
                    options.LogoutPath = ResourceConfiguration.LogoutPath;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(ResourceConfiguration.CookieExpiration);
                });

            return services;
        }
    }
}