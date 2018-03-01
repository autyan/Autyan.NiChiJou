namespace Autyan.NiChiJou.Blog.Models
{
    public class ArticleEditorViewModel
    {
        public long? Id { get; set; }

        public string Title { get; set; }

        public string Extract { get; set; }

        public string Content { get; set; }
    }

    public class PagedArticleQueryViewModel
    {
        public long? BlogId { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }
    }
}
