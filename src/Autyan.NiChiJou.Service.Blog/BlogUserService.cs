using System.Threading.Tasks;
using System.Transactions;
using Autyan.NiChiJou.Core.Mvc.Extension;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.DTO.Blog;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Repository.Blog;
using Autyan.NiChiJou.Service.Blog.ServiceStatusCode;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Autyan.NiChiJou.Service.Blog
{
    public class BlogUserService : BaseService, IBlogUserService
    {
        private readonly IBlogUserRepository _blogUserRepo;

        private readonly IBlogRepository _blogRepo;

        private readonly IDistributedCache _cache;

        public BlogUserService(ILoggerFactory loggerFactory,
            IBlogUserRepository blogUserRepository,
            IBlogRepository blogRepository,
            IDistributedCache cache) : base(loggerFactory)
        {
            _blogUserRepo = blogUserRepository;
            _blogRepo = blogRepository;
            _cache = cache;
        }

        public async Task<ServiceResult<BlogUser>> CreateBlogUserAsync(string nikeName, string memberCode)
        {
            var blogUser = new BlogUser
            {
                AvatorUrl = string.Empty,
                NickName = nikeName,
                MemberCode = memberCode
            };

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var blogUserId = await _blogUserRepo.InsertAsync(blogUser);
                var blogId = await _blogRepo.InsertAsync(new Model.Blog.Blog
                {
                    BlogName = $"{blogUser.NickName}'s Blog",
                    BlogUserId = blogUser.Id
                });
                if (blogUserId <= 0 || blogId <= 0)
                {
                    return Failed<BlogUser>(BlogUserStatus.CreateBlogUserFailed);
                }

                scope.Complete();
            }

            return Success(blogUser);
        }

        public async Task<ServiceResult<BlogUser>> FindBlogUserByMemberCodeAsync(string memberCode)
        {
            var blogUser = await _cache.GetDeserializedAsync<BlogUser>($"memberCode.BlogUser.<{memberCode}>")
                           ?? await _blogUserRepo.FirstOrDefaultAsync(new { MemberCode = memberCode });

            if (blogUser == null)
            {
                return Failed<BlogUser>(BlogUserStatus.BlogUserNotExists);
            }

            return Success(blogUser);
        }

        public async Task<ServiceResult<BlogIdentity>> CreateBlogIdentityAsync(BlogUser user)
        {
            var blog = await _blogRepo.FirstOrDefaultAsync(new {BlogUserId = user.Id});
            return Success(new BlogIdentity
            {
                UserId = user.Id,
                NickName = user.NickName,
                MemberCode = user.MemberCode,
                BlogId = blog?.Id,
                BlogName = blog?.BlogName
            });
        }

        public async Task<ServiceResult<BlogUser>> FindBlogUserByIdAsync(long id)
        {
            var user = await _blogUserRepo.FirstOrDefaultAsync(new BlogUserQuery {Id = id});
            if (user == null)
            {
                return Failed<BlogUser>(BlogUserStatus.BlogUserNotExists);
            }

            return Success(user);
        }
    }
}
