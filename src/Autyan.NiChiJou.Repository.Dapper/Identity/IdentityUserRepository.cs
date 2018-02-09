using System.Linq;
using System.Threading.Tasks;
using Autyan.NiChiJou.Model.Identity;
using Autyan.NiChiJou.Repository.Identity;
using Dapper;

namespace Autyan.NiChiJou.Repository.Dapper.Identity
{
    public class IdentityUserRepository : LongKeyDapperRepository<IdentityUser>, IIdentityUserRepository
    {
        public async Task<IdentityUser> UserRegisteredAsync(IdentityUser user)
        {
            var builder = StartSql();
            builder.SelectCount().FromTable(TableName).WhereAnd("LoginName = @LoginName")
                .WhereOr("Email = @Email").WhereOr("PhoneNumber = @PhoneNumber");

            var users = await Connection.QueryAsync<IdentityUser>(builder.End(), user);
            return users.FirstOrDefault();
        }
    }
}
