using System.Threading.Tasks;
using Autyan.NiChiJou.Blog.Models;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Service.Blog;
using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.Blog.ViewComponents.Article
{
    public class PagedArticleViewComponent : ViewComponent
    {
        private IArticleService ArticleService { get; }

        public PagedArticleViewComponent(IArticleService articleService)
        {
            ArticleService = articleService;
        }

        public async Task<IViewComponentResult> InvokeAsync(PagedArticleQueryViewModel model)
        {
            var pagedArticlePreviews = await ArticleService.GetPagedArticleAsync(new ArticleQuery
            {
                BlogId = model.BlogId,
                Skip = (model.Index - 1) * model.PageSize,
                Take = model.PageSize
            });
            pagedArticlePreviews.Data.Index = model.Index;
            pagedArticlePreviews.Data.PageSize = model.PageSize;
            return View(pagedArticlePreviews.Data);
        }
    }
}
