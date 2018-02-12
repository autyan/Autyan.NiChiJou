using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Identity;

namespace Autyan.NiChiJou.Service.Identity
{
    public interface IIdentityUserService
    {
        Task<ServiceResult<IdentityUser>> GetUserByIdAsync(long id);
    }
}
