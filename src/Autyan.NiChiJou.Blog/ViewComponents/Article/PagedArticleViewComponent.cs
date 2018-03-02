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
                Skip = model.Skip,
                Take = model.Take,
                Desc = new[] {"CreatedAt"}
            });
            pagedArticlePreviews.Data.Skip = model.Skip;
            pagedArticlePreviews.Data.Take = model.Take;
            var viewModel = new PagedArticlePreviewViewModel
            {
                PagedResult = pagedArticlePreviews.Data,
                Route = model.Route,
                RenderPager = model.RenderPager
            };
            return View(viewModel);
        }
    }
}
