using System.Threading.Tasks;
using Autyan.NiChiJou.Blog.Models;
using Autyan.NiChiJou.Core.Context;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Service.Blog;
using Autyan.NiChiJou.Service.DTO.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.Blog.Controllers
{
    public class ArticleController : Controller
    {
        private IArticleService ArticleService { get; }

        private IIdentityContext<BlogIdentity> IdentityContext { get; }

        public ArticleController(IArticleService articleService,
            IIdentityContext<BlogIdentity> identityContext)
        {
            ArticleService = articleService;
            IdentityContext = identityContext;
        }

        [HttpGet("Article/Posts/{id:long}")]
        public async Task<IActionResult> GetArticleAsync(long id)
        {
            var article = await ArticleService.FindArticleAsync(id);
            if (article.Succeed)
            {

            }

            return Redirect("/");
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
                return RedirectToAction(nameof(GetArticleAsync), new {id = result.Data});
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
                    var content = await ArticleService.LoadArticleContent(article.Data.Id.Value);
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

    }
}