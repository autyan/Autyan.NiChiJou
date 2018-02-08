using Autyan.NiChiJou.Core.Data;

namespace Autyan.NiChiJou.Model.Blog
{
    public class Blog : LongKeyBaseEntity
    {
        public virtual string BlogName { get; set; }

        public virtual string Description { get; set; }

        public virtual long? BlogUserId { get; set; }

        public virtual BlogUser BlogUser { get; set; }
    }

    public class BlogQuery : LongKeyBaseEntityQuery
    {
        public string BlogName { get; set; }

        public string Description { get; set; }

        public long? BlogUserId { get; set; }
    }
}
