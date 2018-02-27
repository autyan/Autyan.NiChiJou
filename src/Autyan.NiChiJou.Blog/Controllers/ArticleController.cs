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

        [HttpGet]
        [Route("/Article/{id}")]
        public async Task<IActionResult> GetAsync(ulong id)
        {
            var article = await ArticleService.FindArticleAsync(id);
            if (article.Succeed)
            {

            }

            return Redirect("/");
        }

        [HttpPost]
        [Route("/Article")]
        public async Task<IActionResult> PostAsync(ArticleEditorViewModel model)
        {
            var result = await ArticleService.CreateOrUpdateAsync(new ArticleEdit
            {
                Id = model.Id,
                Title = model.Title,
                Content = model.Content
            });

            if (result.Succeed)
            {
                return RedirectToAction(nameof(GetAsync), new {id = result.Data});
            }

            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Editor() => View();
    }
}