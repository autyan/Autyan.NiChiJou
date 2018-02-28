using System.Threading.Tasks;
using Autyan.NiChiJou.Blog.Models;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Service.Blog;
using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.Blog.Controllers
{
    public class ArticleController : Controller
    {
        private IArticleService ArticleService { get; }

        public ArticleController(IArticleService articleService)
        {
            ArticleService = articleService;
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
        public async Task<IActionResult> Editor([FromBody]ArticleEditorViewModel model)
        {
            var result = await ArticleService.CreateOrUpdateAsync(new Article
            {
                Id = model.Id,
                Title = model.Title,
                Content = model.Content
            });

            if (result.Succeed)
            {
                return RedirectToAction(nameof(GetArticleAsync), new {id = result.Data});
            }

            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Editor() => View();
    }
}