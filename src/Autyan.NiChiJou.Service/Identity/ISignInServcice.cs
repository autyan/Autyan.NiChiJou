using System.Threading.Tasks;
using Autyan.NiChiJou.BusinessModel.Identity;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Identity;

namespace Autyan.NiChiJou.Service.Identity
{
    public interface ISignInServcice
    {
        /// <summary>
        /// register new user.
        /// </summary>
        /// <param name="model">infomation that register needed</param>
        /// <returns>registed user</returns>
        Task<ServiceResult<IdentityUser>> RegisterUserAsync(UserRegisterModel model);

        /// <summary>
        /// register new user, return sessionId.
        /// </summary>
        /// <param name="model">infomation that register needed</param>
        /// <returns>sessionId</returns>
        Task<ServiceResult<string>> RegisterSiginInAsync(UserRegisterModel model);

        /// <summary>
        /// use password sigin in, will get user infomation.
        /// </summary>
        /// <param name="loginName">user loginName</param>
        /// <param name="password">user password</param>
        /// <returns>user infomation</returns>
        Task<ServiceResult<IdentityUser>> PasswordSignInAsync(string loginName, string password);

        /// <summary>
        /// use password sign in, if specified businessCode, will get business domain url.
        /// </summary>
        /// <param name="loginName">user loginName</param>
        /// <param name="password">user password</param>
        /// <param name="businessCode">business that redirect to</param>
        /// <returns>sessionId and business doman url</returns>
        Task<ServiceResult<BusinessSystemSignInModel>> BusinessSystemPasswordSignIn(string loginName, string password, string businessCode);
    }
}
