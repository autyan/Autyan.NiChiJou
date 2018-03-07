using System.Threading.Tasks;
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
        private IBlogUserRepository BlogUserRepo { get; }

        private IBlogRepository BlogRepo { get; }

        private IDistributedCache Cache { get; }

        public BlogUserService(ILoggerFactory loggerFactory,
            IBlogUserRepository blogUserRepository,
            IBlogRepository blogRepository,
            IDistributedCache cache) : base(loggerFactory)
        {
            BlogUserRepo = blogUserRepository;
            BlogRepo = blogRepository;
            Cache = cache;
        }

        public async Task<ServiceResult<BlogUser>> CreateBlogUserAsync(string nikeName, string memberCode)
        {
            var blogUser = new BlogUser
            {
                AvatorUrl = string.Empty,
                NickName = nikeName,
                MemberCode = memberCode
            };

            //using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            //{
            //    var blogUserId = await BlogUserRepo.InsertAsync(blogUser);
            //    var blogId = await BlogRepo.InsertAsync(new Model.Blog.Blog
            //    {
            //        BlogName = $"{blogUser.NickName}'s Blog",
            //        BlogUserId = blogUser.Id
            //    });
            //    if (blogUserId <= 0 || blogId <= 0)
            //    {
            //        return Failed<BlogUser>(BlogUserStatus.CreateBlogUserFailed);
            //    }

            //    scope.Complete();
            //}

            var blogUserId = await BlogUserRepo.InsertAsync(blogUser);
            var blogId = await BlogRepo.InsertAsync(new Model.Blog.Blog
            {
                BlogName = $"{blogUser.NickName}'s Blog",
                BlogUserId = blogUser.Id
            });
            if (blogUserId <= 0 || blogId <= 0)
            {
                return Failed<BlogUser>(BlogUserStatus.CreateBlogUserFailed);
            }

            return Success(blogUser);
        }

        public async Task<ServiceResult<BlogUser>> FindBlogUserByMemberCodeAsync(string memberCode)
        {
            var blogUser = await Cache.GetDeserializedAsync<BlogUser>($"memberCode.BlogUser.<{memberCode}>")
                           ?? await BlogUserRepo.FirstOrDefaultAsync(new { MemberCode = memberCode });

            if (blogUser == null)
            {
                return Failed<BlogUser>(BlogUserStatus.BlogUserNotExists);
            }

            return Success(blogUser);
        }

        public async Task<ServiceResult<BlogIdentity>> CreateBlogIdentityAsync(BlogUser user)
        {
            var blog = await BlogRepo.FirstOrDefaultAsync(new {BlogUserId = user.Id});
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
            var user = await BlogUserRepo.FirstOrDefaultAsync(new BlogUserQuery {Id = id});
            if (user == null)
            {
                return Failed<BlogUser>(BlogUserStatus.BlogUserNotExists);
            }

            return Success(user);
        }
    }
}
