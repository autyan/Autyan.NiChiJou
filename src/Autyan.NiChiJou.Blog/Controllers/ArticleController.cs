using System.Threading.Tasks;
using Autyan.NiChiJou.Blog.Models;
using Autyan.NiChiJou.Core.Context;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Service.Blog;
using Autyan.NiChiJou.Service.DTO.Blog;
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
            var result = await ArticleService.CreateOrUpdateAsync(new Article
            {
                Id = model.Id,
                Title = model.Title,
                Extract = model.Extract,
                Content = model.Content,
                BlogId = IdentityContext.Identity.BlogId
            });

            if (result.Succeed)
            {
                return RedirectToAction(nameof(GetArticleAsync), new {id = result.Data});
            }

            return Redirect("/");
        }

        [HttpGet]
        [Route("Article/Editor/{id:long}")]
        public async Task<IActionResult> Editor(long? id)
        {
            if (id != null)
            {
                var article = await ArticleService.FindArticleAsync(id.Value);
                if (article.Succeed)
                {
                    return View(new ArticleEditorViewModel
                    {
                        Id = id.Value,
                        Title = article.Data.Title,
                        Content = article.Data.Content
                    });
                }
            }
            if (IdentityContext.Identity.BlogId == null)
            {

            }
            return View();
        }
    }
}