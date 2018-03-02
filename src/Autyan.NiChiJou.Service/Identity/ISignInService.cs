using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.DTO.Identity;
using Autyan.NiChiJou.Model.Identity;

namespace Autyan.NiChiJou.Service.Identity
{
    public interface ISignInService
    {
        /// <summary>
        /// register new user.
        /// </summary>
        /// <param name="model">infomation that register needed</param>
        /// <returns>registered user</returns>
        Task<ServiceResult<IdentityUser>> RegisterUserAsync(UserRegistration model);

        /// <summary>
        /// use password sign in, will get user infomation.
        /// </summary>
        /// <param name="loginName">user loginName</param>
        /// <param name="password">user password</param>
        /// <returns>user infomation</returns>
        Task<ServiceResult<IdentityUser>> PasswordSignInAsync(string loginName, string password);
    }
}
