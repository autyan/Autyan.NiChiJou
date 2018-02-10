using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Identity;
using Autyan.NiChiJou.Repository.Identity;

namespace Autyan.NiChiJou.Service.Identity
{
    public class SignInService : BaseService, ISignInServcice
    {
        private IIdentityUserRepository UserRepo { get; }

        public SignInService(IIdentityUserRepository userRepository)
        {
            UserRepo = userRepository;
        }

        public async Task<ServiceResult> RegisterUserAsyc(IdentityUser user)
        {
            var existUser = await UserRepo.UserRegisteredAsync(user);
            if (existUser != null && existUser.LoginName == user.LoginName)
            {
                return ServiceResult.Failed("LoginName exists!", (int)IdentityStatus.LoginNameExists);
            }

            if (existUser != null && existUser.Email == user.Email)
            {
                return ServiceResult.Failed("Email Registered!", (int)IdentityStatus.EmailRegistered);
            }

            if (existUser != null && existUser.PhoneNumber == user.PhoneNumber)
            {
                return ServiceResult.Failed("PhoneNumber Registered!", (int)IdentityStatus.PhoneNumberRegistered);
            }

            await UserRepo.InsertAsync(user);
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> PasswordSignInAsync(string loginName, string password)
        {
            return await Task.FromResult<ServiceResult>(null);
        }
    }
}
