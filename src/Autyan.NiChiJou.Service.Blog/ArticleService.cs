using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Repository.Blog;
using Autyan.NiChiJou.Service.Blog.ServiceStatusCode;
using Autyan.NiChiJou.Service.DTO.Blog;
using Microsoft.Extensions.Logging;

namespace Autyan.NiChiJou.Service.Blog
{
    public class ArticleService : BaseService, IArticleService
    {
        private IArticleRepository ArticleRepo { get; }

        private IArticleCommentRepository CommentRepo { get; }

        public ArticleService(ILoggerFactory loggerFactory,
            IArticleRepository articleRepository,
            IArticleCommentRepository articleCommentRepository) : base(loggerFactory)
        {
            ArticleRepo = articleRepository;
            CommentRepo = articleCommentRepository;
        }

        public Task<ServiceResult<ulong>> CreateOrUpdateAsync(ArticleEdit edit)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceResult<Article>> FindArticleAsync(ulong id)
        {
            var article = await ArticleRepo.FirstOrDefaultAsync(new { Id = id });
            if (article == null)
            {
                return ServiceResult<Article>.Failed(ArticleStatus.ArticleNotFound);
            }

            return ServiceResult<Article>.Success(article);
        }
    }
}
