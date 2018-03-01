using System.Threading.Tasks;
using System.Transactions;
using Autyan.NiChiJou.Core.Data;
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

        private IArticleContentRepository ContentRepo { get; }

        public ArticleService(ILoggerFactory loggerFactory,
            IArticleRepository articleRepository,
            IArticleCommentRepository articleCommentRepository,
            IArticleContentRepository articleContentRepository) : base(loggerFactory)
        {
            ArticleRepo = articleRepository;
            CommentRepo = articleCommentRepository;
            ContentRepo = articleContentRepository;
        }

        public async Task<ServiceResult<Article>> CreateArticleAsync(Article article, string content)
        {
            if (article.Id != null) return Failed<Article>("article exists!");
            using (var scope = new TransactionScope())
            {
                var create = await ArticleRepo.InsertAsync(article);
                if (create <= 0)
                {
                    return Failed<Article>("create article failed");
                }
                create = await ContentRepo.InsertAsync(new ArticleContent
                {
                    ArticleId = create,
                    Content = content
                });
                if (create <= 0)
                {
                    return Failed<Article>("create articleContent failed");
                }
                scope.Complete();
            }
            return Success(article);
        }

        public async Task<ServiceResult<Article>> UpdateArticleAsync(Article article, string content)
        {
            if (article.Id == null) return Failed<Article>("articleId is null");

            using (var scope = new TransactionScope())
            {
                var create = await ArticleRepo.UpdateByIdAsync(article);
                if (create <= 0)
                {
                    return Failed<Article>("update article failed");
                }
                create = await ContentRepo.UpdateByConditionAsync(new ArticleContent
                {
                    ArticleId = create,
                    Content = content
                }, new { ArticleId = article.Id });
                if (create <= 0)
                {
                    return Failed<Article>("update articleContent failed");
                }
                scope.Complete();
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

        public async Task<ServiceResult<PagedResult<Article>>> GetPagedArticleAsync(ArticleQuery query)
        {
            var result = await ArticleRepo.PagingQueryAsync(query);
            return Success(result);
        }

        public async Task<ServiceResult<string>> LoadArticleContent(long id)
        {
            var content = await ContentRepo.FirstOrDefaultAsync(new { ArticleId = id });
            return Success(content?.Content);
        }
    }
}