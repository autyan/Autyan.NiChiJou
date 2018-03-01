using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Data;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Blog;

namespace Autyan.NiChiJou.Service.Blog
{
    public interface IArticleService
    {
        Task<ServiceResult<Article>> CreateArticleAsync(Article article, string content);

        Task<ServiceResult<Article>> UpdateArticleAsync(Article article, string content);

        Task<ServiceResult<Article>> FindArticleAsync(long id);

        Task<ServiceResult<string>> LoadArticleContent(long id);

        Task<ServiceResult<PagedResult<Article>>> GetPagedArticleAsync(ArticleQuery query);
    }
}
