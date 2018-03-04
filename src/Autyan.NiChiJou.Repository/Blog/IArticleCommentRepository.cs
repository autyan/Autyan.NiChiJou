using System.Collections.Generic;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Data;
using Autyan.NiChiJou.DTO.Blog;
using Autyan.NiChiJou.Model.Blog;

namespace Autyan.NiChiJou.Repository.Blog
{
    public interface IArticleCommentRepository : IRepository<ArticleComment>
    {
        Task<IEnumerable<ArticleCommentDetails>> LoadArticleCommentDetailsAsync(ArticleCommentQuery query);
    }
}
