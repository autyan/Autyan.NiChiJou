using Autyan.NiChiJou.Core.Data;

namespace Autyan.NiChiJou.Model.Blog
{
    public class ArticleContent : LongKeyBaseEntity
    {
        public virtual long? ArticleId { get; set; }

        public virtual string Content { get; set; }
    }

    public class ArticleContentQuery : LongKeyBaseEntityQuery
    {
        public long? ArticleId { get; set; }
    }
}
