using Microsoft.Extensions.DependencyInjection;

namespace Autyan.NiChiJou.Service.Identity.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddTransient<ISignInService, SignInService>();
            services.AddTransient<ISessionService, SessionService>();
            services.AddTransient<IMembershipService, MembershipService>();
            services.AddTransient<IIdentityUserService, IdentityUserService>();
            services.AddTransient<IApplicationAuthorizationService, ApplicationAuthorizationService>();
            return services;
        }
    }
}
