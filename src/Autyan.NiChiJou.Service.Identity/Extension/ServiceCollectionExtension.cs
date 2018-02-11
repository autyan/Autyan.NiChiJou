using Microsoft.Extensions.DependencyInjection;

namespace Autyan.NiChiJou.Service.Identity.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddTransient<ISignInServcice, SignInService>();
            return services;
        }
    }
}
