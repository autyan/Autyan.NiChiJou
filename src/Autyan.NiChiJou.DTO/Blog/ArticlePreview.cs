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

        public DateTime? LastReadAt { get; set; }

        public DateTime? LastCommentedAt { get; set; }

        public DateTime? PostedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
