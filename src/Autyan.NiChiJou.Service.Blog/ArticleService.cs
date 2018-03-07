using System;
using System.Linq;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Data;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.DTO.Blog;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Repository.Blog;
using Autyan.NiChiJou.Service.Blog.ServiceStatusCode;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Autyan.NiChiJou.Service.Blog
{
    public class ArticleService : BaseService, IArticleService
    {
        private IArticleRepository ArticleRepo { get; }

        private IArticleCommentRepository CommentRepo { get; }

        private IArticleContentRepository ContentRepo { get; }

        private HttpContext HttpContext { get; }

        private IMemoryCache Cache { get; }

        public ArticleService(ILoggerFactory loggerFactory,
            IArticleRepository articleRepository,
            IArticleCommentRepository articleCommentRepository,
            IArticleContentRepository articleContentRepository,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache cache) : base(loggerFactory)
        {
            ArticleRepo = articleRepository;
            CommentRepo = articleCommentRepository;
            ContentRepo = articleContentRepository;
            HttpContext = httpContextAccessor.HttpContext;
            Cache = cache;
        }

        public async Task<ServiceResult<Article>> CreateArticleAsync(Article article, string content)
        {
            if (article.Id != null) return Failed<Article>("article exists!");
            //using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            //{
            //    var create = await ArticleRepo.InsertAsync(article);
            //    if (create <= 0)
            //    {
            //        return Failed<Article>("create article failed");
            //    }

            //    create = await ContentRepo.InsertAsync(new ArticleContent
            //    {
            //        ArticleId = create,
            //        Content = content
            //    });
            //    if (create <= 0)
            //    {
            //        return Failed<Article>("create articleContent failed");
            //    }

            //    scope.Complete();
            //}

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

            return Success(article);
        }

        public async Task<ServiceResult<Article>> UpdateArticleAsync(Article article, string content)
        {
            if (article.Id == null) return Failed<Article>("articleId is null");

            //using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            //{
            //    var create = await ArticleRepo.UpdateByIdAsync(article);
            //    if (create <= 0)
            //    {
            //        return Failed<Article>("update article failed");
            //    }
            //    create = await ContentRepo.UpdateByConditionAsync(new ArticleContent
            //    {
            //        ArticleId = create,
            //        Content = content
            //    }, new { ArticleId = article.Id });
            //    if (create <= 0)
            //    {
            //        return Failed<Article>("update articleContent failed");
            //    }
            //    scope.Complete();
            //}

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

        public async Task<ServiceResult<PagedResult<ArticlePreview>>> GetPagedArticleAsync(ArticleQuery query)
        {
            var result = await ArticleRepo.QueryPagedArticlePreviewAsync(query);
            return Success(result);
        }

        public async Task<ServiceResult<string>> LoadArticleContentAsync(long id)
        {
            var content = await ContentRepo.FirstOrDefaultAsync(new { ArticleId = id });
            return Success(content?.Content);
        }

        public async Task<ServiceResult<ArticleDetail>> ReadArticleDetailByIdAsync(long id)
        {
            var article = await ArticleRepo.FirstOrDefaultAsync(new ArticleQuery { Id = id });
            if (article == null)
            {
                return Failed<ArticleDetail>(ArticleStatus.ArticleNotFound);
            }

            var content = await ContentRepo.FirstOrDefaultAsync(new ArticleContentQuery { ArticleId = id });
            if (content == null)
            {
                return Failed<ArticleDetail>("Article Content Lost!");
            }

            var comments = await CommentRepo.LoadArticleCommentDetailsAsync(new ArticleCommentQuery
            {
                PostId = id
            });

            if (!Cache.TryGetValue($"article.read.<{id}>.<{HttpContext.Connection.RemoteIpAddress}>", out var _))
            {
                article.ReadCount += 1;
                article.LastReadAt = DateTime.Now;
                await ArticleRepo.UpdateByIdAsync(article);
                Cache.Set($"article.read.<{id}>.<{HttpContext.Connection.RemoteIpAddress}>", "Requested", DateTimeOffset.Now.AddDays(1));
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
            var insertResult = await CommentRepo.InsertAsync(post);
            if (insertResult <= 0)
            {
                return Failed<ArticleCommentDetails>("Insert Comment Failed");
            }

            var article = await ArticleRepo.FirstOrDefaultAsync(new {Id = post.PostId});
            article.Comments += 1;
            await ArticleRepo.UpdateByIdAsync(article);

            var comments = await CommentRepo.LoadArticleCommentDetailsAsync(new ArticleCommentQuery
            {
                Id = insertResult
            });

            return Success(comments.FirstOrDefault());
        }
    }
}