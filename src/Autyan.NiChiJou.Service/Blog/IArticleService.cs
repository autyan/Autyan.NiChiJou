using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Service.DTO.Blog;

namespace Autyan.NiChiJou.Service.Blog
{
    public interface IArticleService
    {
        Task<ServiceResult<ulong>> CreateOrUpdateAsync(ArticleEdit edit);

        Task<ServiceResult<Article>> FindArticleAsync(ulong id);
    }
}
