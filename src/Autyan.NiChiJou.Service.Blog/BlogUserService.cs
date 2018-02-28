﻿using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Mvc.Extension;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Repository.Blog;
using Autyan.NiChiJou.Service.Blog.ServiceStatusCode;
using Autyan.NiChiJou.Service.DTO.Blog;
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

            var createResult = await BlogUserRepo.InsertAsync(blogUser);
            if (createResult <= 0)
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
                NikeName = user.NickName,
                MemberCode = user.MemberCode,
                BlogId = blog?.Id,
                BlogName = blog?.BlogName
            });
        }
    }
}
