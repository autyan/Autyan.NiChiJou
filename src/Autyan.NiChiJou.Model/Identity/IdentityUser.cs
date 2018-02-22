using Autyan.NiChiJou.Core.Data;

namespace Autyan.NiChiJou.Model.Identity
{
    public class IdentityUser : LongKeyBaseEntity
    {
        public virtual string LoginName { get; set; }

        public virtual string UserMemberCode { get; set; }

        public virtual string Email { get; set; }

        public virtual bool? EmailConfirmed { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual bool? PhoneNumberConfirmed { get; set; }

        public virtual string PasswordHash { get; set; }

        public virtual string SecuritySalt { get; set; }
    }

    public class IdentityUserQuery : LongKeyBaseEntityQuery
    {
        public string LoginName { get; set; }

        public string Email { get; set; }

        public bool? EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool? PhoneNumberConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecuritySalt { get; set; }
    }
}
