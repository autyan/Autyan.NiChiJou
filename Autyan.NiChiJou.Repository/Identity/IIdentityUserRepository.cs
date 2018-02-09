using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Data;
using Autyan.NiChiJou.Model.Identity;

namespace Autyan.NiChiJou.Repository.Identity
{
    public interface IIdentityUserRepository : IRepository<IdentityUser>
    {
        Task<IdentityUser> UserRegisteredAsync(IdentityUser user);
    }
}
