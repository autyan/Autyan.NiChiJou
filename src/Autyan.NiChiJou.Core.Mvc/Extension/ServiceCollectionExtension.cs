using System;
using Autyan.NiChiJou.Core.Config;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Autyan.NiChiJou.Core.Mvc.Extension
{
    public static class ServiceCollectionExtension
    {
        public static AuthenticationBuilder AddCookieAuthentication(this AuthenticationBuilder builder)
        {
            builder.AddCookie(ResourceConfiguration.CookieAuthenticationScheme, options =>
                {
                    options.LoginPath = ResourceConfiguration.LoginPath;
                    options.LogoutPath = ResourceConfiguration.LogoutPath;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(ResourceConfiguration.CookieExpiration);
                });

            return builder;
        }

        public static AuthenticationBuilder AddServiceTokenAuthentication(this AuthenticationBuilder builder)
        {
            builder.AddServiceToken(ResourceConfiguration.ServiceTokenAuthenticationScheme,
                options =>
                {
                    options.AuthenticationSchema = ResourceConfiguration.ServiceTokenAuthenticationScheme;
                    options.AllowedApps = ResourceConfiguration.ServiceTokenAllowedApps;
                    options.RequestMaxAgeSeconds = ResourceConfiguration.ServiceTokenMaxAge;
                });
            return builder;
        }
    }
}