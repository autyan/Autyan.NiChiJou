using Microsoft.Extensions.DependencyInjection;

namespace Autyan.NiChiJou.Model.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddNiChiJouDataModel(this IServiceCollection services)
        {
            return services;
        }
    }
}
