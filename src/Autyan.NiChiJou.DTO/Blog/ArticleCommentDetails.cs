using System;

namespace Autyan.NiChiJou.DTO.Blog
{
    public class ArticleCommentDetails
    {
        public string Content { get; set; }

        public string CommentedBy { get; set; }

        public long? PostId { get; set; }

        public long? ToComment { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }
    }
}
