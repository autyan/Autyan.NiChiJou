using Microsoft.Extensions.DependencyInjection;

namespace Autyan.NiChiJou.Service.Blog.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBlogService(this IServiceCollection services)
        {
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<IArticleService, ArticleService>();
            return services;
        }
    }
}
