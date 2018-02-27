using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Service.DTO.Blog;
using Microsoft.Extensions.Logging;

namespace Autyan.NiChiJou.Service.Blog
{
    public class ArticleService : BaseService, IArticleService
    {
        public ArticleService(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        public Task<ServiceResult<ulong>> CreateOrUpdateAsync(ArticleEdit edit)
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceResult<Article>> FindArticleAsync(ulong id)
        {
            throw new System.NotImplementedException();
        }
    }
}
