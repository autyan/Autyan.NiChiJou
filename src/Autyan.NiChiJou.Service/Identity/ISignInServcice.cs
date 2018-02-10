using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Identity;

namespace Autyan.NiChiJou.Service.Identity
{
    public interface ISignInServcice
    {
        Task<ServiceResult> RegisterUserAsyc(IdentityUser user);

        Task<ServiceResult> PasswordSignInAsync(string loginName, string password);
    }
}
