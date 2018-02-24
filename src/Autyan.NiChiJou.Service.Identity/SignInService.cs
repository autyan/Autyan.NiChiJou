using System;
using System.Threading.Tasks;
using Autyan.NiChiJou.BusinessModel.Identity;
using Autyan.NiChiJou.Core.Component;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Identity;
using Autyan.NiChiJou.Repository.Identity;

namespace Autyan.NiChiJou.Service.Identity
{
    public class SignInService : BaseService, ISignInService
    {
        private IIdentityUserRepository UserRepo { get; }

        public SignInService(IIdentityUserRepository userRepository)
        {
            UserRepo = userRepository;
        }

        public async Task<ServiceResult<IdentityUser>> RegisterUserAsync(UserRegisterModel model)
        {
            var existUser = await UserRepo.FirstOrDefaultAsync(new { model.LoginName });
            if (existUser != null)
            {
                return ServiceResult<IdentityUser>.Failed("LoginName exists!", (int)IdentityStatus.LoginNameExists);
            }

            var salt = Guid.NewGuid().ToString().ToLower();
            var passwordHash = HashEncrypter.Sha256Encrypt(model.Password, salt);
            var user = new IdentityUser
            {
                LoginName = model.LoginName,
                UserMemberCode = Guid.NewGuid().ToString().ToUpper(),
                NickName = model.LoginName,
                PasswordHash = passwordHash,
                SecuritySalt = salt,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false
            };
            var id = await UserRepo.InsertAsync(user);
            user = await UserRepo.GetByIdAsync(new IdentityUser
            {
                Id = id
            });
            return ServiceResult<IdentityUser>.Success(user);
        }

        public async Task<ServiceResult<IdentityUser>> PasswordSignInAsync(string loginName, string password)
        {
            var user = await UserRepo.FirstOrDefaultAsync(new { LoginName = loginName });
            if (user == null)
            {
                return ServiceResult<IdentityUser>.Failed("LoginName exists!", (int)IdentityStatus.UserNotFound);
            }

            var computedHash = HashEncrypter.Sha256Encrypt(password, user.SecuritySalt);
            return computedHash != user.PasswordHash
                ? ServiceResult<IdentityUser>.Failed("LoginName exists!", (int)IdentityStatus.InvalidPassword)
                : ServiceResult<IdentityUser>.Success(user);
        }
    }
}
