using System.Threading.Tasks;
using Autyan.NiChiJou.Blog.Models;
using Autyan.NiChiJou.Core.Context;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.DTO.Blog;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Service.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Autyan.NiChiJou.Blog.Controllers
{
    public class ArticleController : Controller
    {
        private IArticleService ArticleService { get; }

        private IIdentityContext<BlogIdentity> IdentityContext { get; }

        private ILogger Logger { get; }

        public ArticleController(IArticleService articleService,
            IIdentityContext<BlogIdentity> identityContext,
            ILoggerFactory loggerFactory)
        {
            ArticleService = articleService;
            IdentityContext = identityContext;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        [HttpGet("Article/Posts/{id:long}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticleAsync(long id)
        {
            var article = await ArticleService.ReadArticleDetailByIdAsync(id);
            if (article.Succeed)
            {
                return View(nameof(Article), article.Data);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Editor(ArticleEditorViewModel model)
        {
            ServiceResult<Article> result;
            if (model.Id == null)
            {
                result = await ArticleService.CreateArticleAsync(new Article
                {
                    Id = model.Id,
                    Title = model.Title,
                    Extract = model.Extract,
                    BlogId = IdentityContext.Identity.BlogId
                }, model.Content);
            }
            else
            {
                result = await ArticleService.UpdateArticleAsync(new Article
                {
                    Id = model.Id,
                    Title = model.Title,
                    Extract = model.Extract,
                    BlogId = IdentityContext.Identity.BlogId
                }, model.Content);
            }

            if (result.Succeed)
            {
                return Redirect($"/Article/Posts/{result.Data.Id}");
            }

            return Redirect("/");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Article/Editor/{id:long}")]
        public async Task<IActionResult> Editor(long? id)
        {
            if (id != null)
            {
                var article = await ArticleService.FindArticleAsync(id.Value);
                if (article.Succeed && article.Data.Id != null)
                {
                    var model = new ArticleEditorViewModel
                    {
                        Id = id.Value,
                        Title = article.Data.Title
                    };
                    var content = await ArticleService.LoadArticleContentAsync(article.Data.Id.Value);
                    model.Content = content.Data;
                    return View(model);
                }

                return NotFound();
            }
            return View();
        }

        [HttpGet]
        [Route("Article/Editor")]
        public IActionResult Editor()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Comment(AddCommentViewModel model)
        {
            var comment = new ArticleComment
            {
                PostId = model.ArticleId,
                Content = model.Content
            };
            if (IdentityContext.Identity != null)
            {
                comment.CommentedBy = IdentityContext.Identity.UserId;
            }

            var result = await ArticleService.AddCommentOnArticle(comment);
            if (!result.Succeed)
            {
                return Json("Failed");
            }
            return Json(result.Data);
        }
    }
}