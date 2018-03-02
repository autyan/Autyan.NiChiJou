using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.DTO.Blog;
using Autyan.NiChiJou.Repository.Blog;
using Autyan.NiChiJou.Service.Blog.ServiceStatusCode;
using Microsoft.Extensions.Logging;

namespace Autyan.NiChiJou.Service.Blog
{
    public class BlogService : BaseService, IBlogService
    {
        private IBlogRepository BlogRepo { get; }

        public BlogService(ILoggerFactory loggerFactory,
            IBlogRepository blogRepository) : base(loggerFactory)
        {
            BlogRepo = blogRepository;
        }

        public async Task<ServiceResult<BlogIndex>> LoadBlogByNameAsync(string blogName)
        {
            var blog = await BlogRepo.FirstOrDefaultAsync(new { BlogName = blogName });
            if (blog == null) return Failed<BlogIndex>(BlogStatus.BlogNotFound);

            return Success(new BlogIndex
            {
                BlogUserId = blog.BlogUserId,
                BlogId = blog.Id,
                BlogName = blogName
            });
        }
    }
}
