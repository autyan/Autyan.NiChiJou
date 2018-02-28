using Autyan.NiChiJou.Model.Identity;

namespace Autyan.NiChiJou.Repository.Dapper.Identity.Configuration
{
    public class IdentityUserConfiguration : DatabaseItemConfiguration<IdentityUser>
    {
        public IdentityUserConfiguration()
        {
            this["LoginName"].HasMaxLength(50);
            this["NickName"].HasMaxLength(50);
            this["UserMemberCode"].HasMaxLength(50);
            this["Email"].HasMaxLength(50);
            this["PhoneNumber"].HasMaxLength(50);
            this["PasswordHash"].HasMaxLength(200);
            this["SecuritySalt"].HasMaxLength(200);
        }
    }
}
