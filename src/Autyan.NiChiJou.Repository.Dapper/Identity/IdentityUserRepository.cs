using Autyan.NiChiJou.Model.Identity;
using Autyan.NiChiJou.Repository.Identity;

namespace Autyan.NiChiJou.Repository.Dapper.Identity
{
    public class IdentityUserRepository : LongKeyDapperRepository<IdentityUser>, IIdentityUserRepository
    {
    }
}
