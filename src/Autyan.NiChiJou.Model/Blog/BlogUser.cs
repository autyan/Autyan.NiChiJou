using Autyan.NiChiJou.Core.Data;
using Autyan.NiChiJou.Model.Enumeration;

namespace Autyan.NiChiJou.Model.Blog
{
    public class BlogUser : LongKeyBaseEntity
    {
        public virtual string NickName { get; set; }

        public virtual string AvatorUrl { get; set; }

        public virtual string MemberCode { get; set; }

        public virtual Gender? Gender { get; set; }
    }

    public class BlogUserQuery : LongKeyBaseEntityQuery
    {
        public string NickName { get; set; }

        public string AvatorUrl { get; set; }

        public string MemberCode { get; set; }

        public Gender Gender { get; set; }
    }
}
