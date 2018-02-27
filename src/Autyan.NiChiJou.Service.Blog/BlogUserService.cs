using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Mvc.Extension;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Repository.Blog;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Autyan.NiChiJou.Service.Blog
{
    public class BlogUserService : BaseService, IBlogUserService
    {
        private IBlogUserRepository BlogUserRepo { get; }

        private IDistributedCache Cache { get; }

        public BlogUserService(ILoggerFactory loggerFactory,
            IBlogUserRepository blogUserRepository,
            IDistributedCache cache) : base(loggerFactory)
        {
            BlogUserRepo = blogUserRepository;
            Cache = cache;
        }

        public async Task<ServiceResult<BlogUser>> CreateBlogUserAsync(string nickName, string memberCode)
        {
            var blogUser = new BlogUser
            {
                AvatorUrl = string.Empty,
                NickName = nickName,
                UserMemberCode = memberCode
            };

            var createResult = await BlogUserRepo.InsertAsync(blogUser);
            if (createResult <= 0)
            {
                return Failed<BlogUser>("Create BlogUser Failed");
            }

            return Success(blogUser);
        }

        public async Task<ServiceResult<BlogUser>> FindBlogUserByMemberCodeAsync(string memberCode)
        {
            var blogUser = await Cache.GetDeserializedAsync<BlogUser>($"memberCode.BlogUser.<{memberCode}>") 
                           ?? await BlogUserRepo.FirstOrDefaultAsync(new { UserMemberCode = memberCode });

            if (blogUser == null)
            {
                return Failed<BlogUser>("blog user not exist!");
            }

            return Success(blogUser);
        }
    }
}
