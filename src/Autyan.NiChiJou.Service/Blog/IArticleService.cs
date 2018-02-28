using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Blog;

namespace Autyan.NiChiJou.Service.Blog
{
    public interface IArticleService
    {
        Task<ServiceResult<Article>> CreateOrUpdateAsync(Article article);

        Task<ServiceResult<Article>> FindArticleAsync(long id);
    }
}
