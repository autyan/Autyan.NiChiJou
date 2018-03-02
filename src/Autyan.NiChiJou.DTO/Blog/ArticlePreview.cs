using System;

namespace Autyan.NiChiJou.DTO.Blog
{
    public class ArticlePreview
    {
        public long? Id { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string Extract { get; set; }

        public long? BlogId { get; set; }

        public string BlogName { get; set; }

        public long? Reads { get; set; }

        public long? Comments { get; set; }

        public DateTimeOffset? LastReadAt { get; set; }

        public DateTimeOffset? LastCommentedAt { get; set; }

        public DateTimeOffset? PostedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
