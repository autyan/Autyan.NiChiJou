using Autyan.NiChiJou.Core.Data;
using Autyan.NiChiJou.DTO.Blog;

namespace Autyan.NiChiJou.Blog.Models
{
    public class ArticleEditorViewModel
    {
        public long? Id { get; set; }

        public string Title { get; set; }

        public string Extract { get; set; }

        public string Content { get; set; }
    }

    public class PagedArticleQueryViewModel : PagedQueryViewModel
    {
        public long? BlogId { get; set; }

        public bool RenderPager { get; set; }

        public RouteViewModel Route { get; set; } = new RouteViewModel();
    }

    public class PagedArticlePreviewViewModel
    {
        public PagedResult<ArticlePreview> PagedResult { get; set; }

        public RouteViewModel Route { get; set; }

        public bool RenderPager { get; set; }
    }
}
