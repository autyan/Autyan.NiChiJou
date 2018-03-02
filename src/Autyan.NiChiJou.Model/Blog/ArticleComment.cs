using Autyan.NiChiJou.Core.Data;

namespace Autyan.NiChiJou.Model.Blog
{
    public class ArticleComment : LongKeyBaseEntity
    {
        public virtual string Content { get; set; }

        public virtual string CommentedBy { get; set; }

        public virtual long? PostId { get; set; }

        public virtual Article Article { get; set; }

        public virtual long? ToComment { get; set; }
    }

    public class PostCommentQuery : LongKeyBaseEntityQuery
    {
        public string Content { get; set; }

        public long? BlogUserId { get; set; }


        public long? PostId { get; set; }


        public long? ToComment { get; set; }
    }
}
