using System.ComponentModel.DataAnnotations;
using Autyan.NiChiJou.Core.Data;
using Autyan.NiChiJou.DTO.Blog;

namespace Autyan.NiChiJou.Blog.Models
{
    public class ArticleEditorViewModel
    {
        public long? Id { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(500)]
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
