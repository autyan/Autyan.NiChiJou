using System;
using Autyan.NiChiJou.Core.Repository;
using Autyan.NiChiJou.Core.Repository.DbConnectionFactory;
using Autyan.NiChiJou.Core.Utility.Sql;
using Autyan.NiChiJou.Repository.Blog;
using Autyan.NiChiJou.Repository.Dapper.Blog;
using Autyan.NiChiJou.Repository.Dapper.Identity;
using Autyan.NiChiJou.Repository.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Autyan.NiChiJou.Repository.Dapper.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDapper(this IServiceCollection services)
        {
            //load model Metadata
            MetadataContext.Instance.Initilize(AppDomain.CurrentDomain.GetAssemblies());

            //default database use mssql
            UseDapperWithMySql(services);

            services.AddScoped<IIdentityUserRepository, IdentityUserRepository>();
            services.AddScoped<IServiceTokenRepository, ServiceTokenRepository>();
            services.AddScoped<IBlogUserRepository, BlogUserRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IArticleCommentRepository, ArticleCommentRepository>();
            services.AddScoped<IArticleContentRepository, ArticleContentRepository>();
            return services;
        }

        public static IServiceCollection UseDapperWithMsSql(this IServiceCollection services)
        {
            services.AddSingleton<ISqlBuilderFactory, MsSqlBuilderFactory>();
            services.AddSingleton<IDbConnectionFactory, MsSqlDbConnectionFactory>();
            return services;
        }

        public static IServiceCollection UseDapperWithMySql(this IServiceCollection services)
        {
            services.AddSingleton<ISqlBuilderFactory, MySqlBuilderFactory>();
            services.AddSingleton<IDbConnectionFactory, MySqlDbConnectionFactory>();
            return services;
        }
    }
}
