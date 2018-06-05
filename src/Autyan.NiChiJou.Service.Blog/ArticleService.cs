using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;
using Autyan.NiChiJou.Core.Data;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.DTO.Blog;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Repository.Blog;
using Autyan.NiChiJou.Service.Blog.ServiceStatusCode;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Autyan.NiChiJou.Service.Blog
{
    public class ArticleService : BaseService, IArticleService
    {
        private readonly IArticleRepository _articleRepo;

        private readonly IArticleCommentRepository _commentRepo;

        private readonly IArticleContentRepository _contentRepo;

        private readonly IMemoryCache _cache;

        public ArticleService(ILoggerFactory loggerFactory,
            IArticleRepository articleRepository,
            IArticleCommentRepository articleCommentRepository,
            IArticleContentRepository articleContentRepository,
            IMemoryCache cache) : base(loggerFactory)
        {
            _articleRepo = articleRepository;
            _commentRepo = articleCommentRepository;
            _contentRepo = articleContentRepository;
            _cache = cache;
        }

        public async Task<ServiceResult<Article>> CreateArticleAsync(Article article, string content)
        {
            if (article.Id != null) return Failed<Article>("article exists!");
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var create = await _articleRepo.InsertAsync(article);
                if (create <= 0)
                {
                    return Failed<Article>("create article failed");
                }

                create = await _contentRepo.InsertAsync(new ArticleContent
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

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var create = await _articleRepo.PartialUpdateByIdAsync(article);
                if (create <= 0)
                {
                    return Failed<Article>("update article failed");
                }
                create = await _contentRepo.UpdateByConditionAsync(new
                {
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
            var article = await _articleRepo.FirstOrDefaultAsync(new { Id = id });
            if (article == null)
            {
                return Failed<Article>(ArticleStatus.ArticleNotFound);
            }

            return Success(article);
        }

        public async Task<ServiceResult<PagedResult<ArticlePreview>>> GetPagedArticleAsync(ArticleQuery query)
        {
            var result = await _articleRepo.QueryPagedArticlePreviewAsync(query);
            return Success(result);
        }

        public async Task<ServiceResult<string>> LoadArticleContentAsync(long id)
        {
            var content = await _contentRepo.FirstOrDefaultAsync(new { ArticleId = id });
            return Success(content?.Content);
        }

        public async Task<ServiceResult<ArticleDetail>> ReadArticleDetailByIdAsync(long id, IPAddress ipAddress)
        {
            var article = await _articleRepo.FirstOrDefaultAsync(new ArticleQuery { Id = id });
            if (article == null)
            {
                return Failed<ArticleDetail>(ArticleStatus.ArticleNotFound);
            }

            var content = await _contentRepo.FirstOrDefaultAsync(new ArticleContentQuery { ArticleId = id });
            if (content == null)
            {
                return Failed<ArticleDetail>("Article Content Lost!");
            }

            var comments = await _commentRepo.LoadArticleCommentDetailsAsync(new ArticleCommentQuery
            {
                PostId = id
            });

            if (!_cache.TryGetValue($"article.read.<{id}>.<{ipAddress}>", out var _))
            {
                article.ReadCount += 1;
                article.LastReadAt = DateTime.Now;
                await _articleRepo.UpdateByIdAsync(article);
                _cache.Set($"article.read.<{id}>.<{ipAddress}>", "Requested", DateTimeOffset.Now.AddDays(1));
            }

            return Success(new ArticleDetail
            {
                Id = article.Id,
                BlogId = article.BlogId,
                Title = article.Title,
                Content = content.Content,
                Comments = comments
            });
        }

        public async Task<ServiceResult<ArticleCommentDetails>> AddCommentOnArticle(ArticleComment post)
        {
            var insertResult = await _commentRepo.InsertAsync(post);
            if (insertResult <= 0)
            {
                return Failed<ArticleCommentDetails>("Insert Comment Failed");
            }

            var article = await _articleRepo.FirstOrDefaultAsync(new {Id = post.PostId});
            article.Comments += 1;
            await _articleRepo.UpdateByIdAsync(article);

            var comments = await _commentRepo.LoadArticleCommentDetailsAsync(new ArticleCommentQuery
            {
                Id = insertResult
            });

            return Success(comments.FirstOrDefault());
        }
    }
}