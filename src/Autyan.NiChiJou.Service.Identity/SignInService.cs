using System;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Component;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.DTO.Identity;
using Autyan.NiChiJou.Model.Identity;
using Autyan.NiChiJou.Repository.Identity;
using Autyan.NiChiJou.Service.Identity.ServiceStatusCode;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Autyan.NiChiJou.Service.Identity
{
    public class SignInService : BaseService, ISignInService
    {
        private IIdentityUserRepository UserRepo { get; }

        private IDistributedCache Cache { get; }

        public SignInService(IIdentityUserRepository userRepository,
            ILoggerFactory loggerFactory,
            IDistributedCache cache): base(loggerFactory)
        {
            UserRepo = userRepository;
            Cache = cache;
        }

        public async Task<ServiceResult<IdentityUser>> RegisterUserAsync(UserRegistration model)
        {
            if (Cache.GetStringAsync($"Identity.Registration.InviteCode{model.InviteCode}") == null)
            {
                return Failed<IdentityUser>(IdentityStatus.InvalidInviteCode);
            }
            var existUser = await UserRepo.FirstOrDefaultAsync(new { model.LoginName });
            if (existUser != null)
            {
                return Failed<IdentityUser>(IdentityStatus.LoginNameExists);
            }

            var salt = Guid.NewGuid().ToString().ToLower();
            var passwordHash = HashEncrypter.Sha256Encrypt(model.Password, salt);
            var user = new IdentityUser
            {
                LoginName = model.LoginName,
                MemberCode = Guid.NewGuid().ToString().ToUpper(),
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
            return Success(user);
        }

        public async Task<ServiceResult<IdentityUser>> PasswordSignInAsync(string loginName, string password)
        {
            var user = await UserRepo.FirstOrDefaultAsync(new { LoginName = loginName });
            if (user == null)
            {
                return Failed<IdentityUser>(IdentityStatus.UserNotFound);
            }

            var computedHash = HashEncrypter.Sha256Encrypt(password, user.SecuritySalt);
            return computedHash != user.PasswordHash
                ? Failed<IdentityUser>(IdentityStatus.InvalidPassword)
                : Success(user);
        }
    }
}
