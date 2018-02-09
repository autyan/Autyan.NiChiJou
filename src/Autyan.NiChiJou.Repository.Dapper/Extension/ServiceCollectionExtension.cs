using Autyan.NiChiJou.Core.Repository.Blog;
using Autyan.NiChiJou.Repository.Dapper.Blog;
using Autyan.NiChiJou.Repository.Dapper.Identity;
using Autyan.NiChiJou.Repository.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Autyan.NiChiJou.Repository.Dapper.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection UseDapper(this IServiceCollection services)
        {
            //Default database use mssql
            UseDapperWithMsSql(services);

            services.AddTransient<IIdentityUserRepository, IdentityUserRepository>();
            services.AddTransient<IBlogUserRepository, BlogUserRepository>();
            return services;
        }

        public static IServiceCollection UseDapperWithMsSql(this IServiceCollection services)
        {
            DapperConfiguration.UseMssql();
            return services;
        }

        public static IServiceCollection UseDapperWithMySql(this IServiceCollection services)
        {
            DapperConfiguration.UseMySql();
            return services;
        }
    }
}
