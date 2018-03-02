using System.Collections.Generic;
using Autyan.NiChiJou.Model.Blog;

namespace Autyan.NiChiJou.DTO.Blog
{
    public class ArticleDetail
    {
        public long? Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public long? BlogId { get; set; }

        public IEnumerable<ArticleComment> Comments { get; set; }
    }
}
