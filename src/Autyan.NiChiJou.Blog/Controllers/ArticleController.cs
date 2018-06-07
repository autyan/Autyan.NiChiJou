using System.Threading.Tasks;
using Autyan.NiChiJou.Blog.Models;
using Autyan.NiChiJou.Core.Context;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.DTO.Blog;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Service.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.Blog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        private readonly IIdentityContext<BlogIdentity> _identityContext;

        public ArticleController(IArticleService articleService,
            IIdentityContext<BlogIdentity> identityContext)
        {
            _articleService = articleService;
            _identityContext = identityContext;
        }

        [HttpGet("Article/Posts/{id:long}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticleAsync(long id)
        {
            var article = await _articleService.ReadArticleDetailByIdAsync(id, HttpContext.Connection.RemoteIpAddress);
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
                result = await _articleService.CreateArticleAsync(new Article
                {
                    Id = model.Id,
                    Title = model.Title,
                    Extract = model.Extract,
                    BlogId = _identityContext.Identity.BlogId
                }, model.Content);
            }
            else
            {
                result = await _articleService.UpdateArticleAsync(new Article
                {
                    Id = model.Id,
                    Title = model.Title,
                    Extract = model.Extract,
                    BlogId = _identityContext.Identity.BlogId
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
                var article = await _articleService.FindArticleAsync(id.Value);
                if (article.Succeed && article.Data.Id != null)
                {
                    var model = new ArticleEditorViewModel
                    {
                        Id = id.Value,
                        Title = article.Data.Title
                    };
                    var content = await _articleService.LoadArticleContentAsync(article.Data.Id.Value);
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
            if (_identityContext.Identity != null)
            {
                comment.CommentedBy = _identityContext.Identity.UserId;
            }

            var result = await _articleService.AddCommentOnArticle(comment, _identityContext.Identity?.MemberCode, HttpContext.Connection.RemoteIpAddress);
            if (!result.Succeed)
            {
                return Json(result);
            }
            return Json(result.Data);
        }
    }
}