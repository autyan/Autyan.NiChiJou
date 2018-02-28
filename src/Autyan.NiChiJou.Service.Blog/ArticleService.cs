using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Repository.Blog;
using Autyan.NiChiJou.Service.Blog.ServiceStatusCode;
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

        public async Task<ServiceResult<Article>> CreateOrUpdateAsync(Article article)
        {
            long executeResult;
            if (article.Id == null)
            {
                executeResult = await ArticleRepo.InsertAsync(article);
            }
            else
            {
                executeResult = await ArticleRepo.UpdateByIdAsync(article);
            }

            if (executeResult <= 0)
            {
                return Failed<Article>("create or update failed");
            }

            return Success(article);
        }

        public async Task<ServiceResult<Article>> FindArticleAsync(long id)
        {
            var article = await ArticleRepo.FirstOrDefaultAsync(new { Id = id });
            if (article == null)
            {
                return Failed<Article>(ArticleStatus.ArticleNotFound);
            }

            return Success(article);
        }
    }
}
