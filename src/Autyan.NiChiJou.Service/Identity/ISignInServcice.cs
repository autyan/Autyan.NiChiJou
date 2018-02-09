using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Identity;

namespace Autyan.NiChiJou.Service.Identity
{
    public interface ISignInServcice
    {
        Task<ServiceResult> RegisterUser(IdentityUser user);
    }
}
