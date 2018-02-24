using System;
using Autyan.NiChiJou.Core.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Autyan.NiChiJou.Core.Mvc.Extension
{
    public static class ServiceTokenExtension
    {
        public static AuthenticationBuilder AddServiceToken(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<ServiceTokenAuthenticationOptions> configureOptions)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<ServiceTokenAuthenticationOptions>, PostConfigureServiceTokenAuthenticationOptions>());
            return builder.AddScheme<ServiceTokenAuthenticationOptions, ServiceTokenAuthenticationhandler>(authenticationScheme, displayName, configureOptions);
        }

        public static AuthenticationBuilder AddServiceToken(this AuthenticationBuilder builder, string authenticationScheme, Action<ServiceTokenAuthenticationOptions> configureOptions)
        {
            return builder.AddServiceToken(authenticationScheme, null, configureOptions);
        }
    }
}
