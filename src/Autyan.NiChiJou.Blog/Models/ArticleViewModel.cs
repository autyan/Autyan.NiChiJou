namespace Autyan.NiChiJou.Blog.Models
{
    public class ArticleViewModel
    {
    }

    public class ArticleEditorViewModel
    {
        public long? Id { get; set; }

        public string Title { get; set; }

        public string Extract { get; set; }

        public string Content { get; set; }
    }
}
