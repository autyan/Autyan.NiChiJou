using System.Threading.Tasks;
using Autyan.NiChiJou.Blog.Models;
using Autyan.NiChiJou.Service.Blog;
using Autyan.NiChiJou.Service.DTO.Blog;
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

        [HttpGet("Article/Posts/{id}")]
        public async Task<IActionResult> GetArticleAsync(ulong id)
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
            var result = await ArticleService.CreateOrUpdateAsync(new ArticleEdit
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