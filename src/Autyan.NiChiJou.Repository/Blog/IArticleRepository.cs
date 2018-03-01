using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Data;
using Autyan.NiChiJou.DTO.Blog;
using Autyan.NiChiJou.Model.Blog;

namespace Autyan.NiChiJou.Repository.Blog
{
    public interface IArticleRepository : IRepository<Article>
    {
        Task<PagedResult<ArticlePreview>> QueryPagedArticlePreviewAsync(ArticleQuery query);
    }
}
