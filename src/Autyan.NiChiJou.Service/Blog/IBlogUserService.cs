using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Service.DTO.Blog;

namespace Autyan.NiChiJou.Service.Blog
{
    public interface IBlogUserService
    {
        Task<ServiceResult<BlogUser>> CreateBlogUserAsync(string nikeName, string memberCode);

        Task<ServiceResult<BlogUser>> FindBlogUserByMemberCodeAsync(string memberCode);

        Task<ServiceResult<BlogIdentity>> CreateBlogIdentityAsync(BlogUser user);
    }
}
