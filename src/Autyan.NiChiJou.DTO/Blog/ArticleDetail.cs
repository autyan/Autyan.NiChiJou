using System.Collections.Generic;

namespace Autyan.NiChiJou.DTO.Blog
{
    public class ArticleDetail
    {
        public long? Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public long? BlogId { get; set; }

        public IEnumerable<ArticleCommentDetails> Comments { get; set; }
    }
}
