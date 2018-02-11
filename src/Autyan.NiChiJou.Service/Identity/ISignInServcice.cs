using System.Threading.Tasks;
using Autyan.NiChiJou.BusinessModel.Identity;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Identity;

namespace Autyan.NiChiJou.Service.Identity
{
    public interface ISignInServcice
    {
        Task<ServiceResult> RegisterUserAsync(UserRegisterModel model);

        Task<ServiceResult<IdentityUser>> PasswordSignInAsync(string loginName, string password);
    }
}
