using Autyan.NiChiJou.Core.Data;
using Autyan.NiChiJou.Model.Enumeration;
using Autyan.NiChiJou.Model.Identity;

namespace Autyan.NiChiJou.Model.Blog
{
    public class BlogUser : LongKeyBaseEntity
    {
        public virtual string NickName { get; set; }

        public virtual string AvatorUrl { get; set; }

        public virtual long? LoginUserId { get; set; }

        public virtual UserRegisterModel IdentityUser { get; set; }

        public virtual Gender Gender { get; set; }
    }

    public class BlogUserQuery : LongKeyBaseEntityQuery
    {
        public string NickName { get; set; }

        public string AvatorUrl { get; set; }

        public long? LoginUserId { get; set; }

        public Gender Gender { get; set; }
    }
}
