using Autyan.NiChiJou.Core.Repository.Identity;
using Autyan.NiChiJou.Model.Identity;

namespace Autyan.NiChiJou.Repository.Dapper.Identity
{
    public class IdentityUserRepository : LongKeyDapperRepository<IdentityUser>, IIdentityUserRepository
    {
    }
}
