using Autyan.NiChiJou.Core.Data;

namespace Autyan.NiChiJou.Model.Blog
{
    public class BlogPost : LongKeyBaseEntity
    {
        public virtual string Title { get; set; }

        public virtual string Extract { get; set; }

        public virtual byte[] Content { get; set; }

        public virtual long? BlogId { get; set; }

        public virtual Blog Blog { get; set; }
    }

    public class BlogPostQuery : LongKeyBaseEntityQuery
    {
        public string Title { get; set; }

        public string Extract { get; set; }

        public long? BlogId { get; set; }
    }
}
