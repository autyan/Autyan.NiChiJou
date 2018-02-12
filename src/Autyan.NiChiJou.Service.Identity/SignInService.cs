using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Autyan.NiChiJou.BusinessModel.Identity;
using Autyan.NiChiJou.Core.Component;
using Autyan.NiChiJou.Core.Config;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Identity;
using Autyan.NiChiJou.Repository.Identity;

namespace Autyan.NiChiJou.Service.Identity
{
    public class SignInService : BaseService, ISignInServcice
    {
        private IIdentityUserRepository UserRepo { get; }

        private IBusinessSystemRepository BuSysRepo { get; }

        private ISessionService SessionService { get; }

        public SignInService(IIdentityUserRepository userRepository,
            IBusinessSystemRepository buSysRepo,
            ISessionService sessionService)
        {
            UserRepo = userRepository;
            BuSysRepo = buSysRepo;
            SessionService = sessionService;
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
                PasswordHash = passwordHash,
                SecuritySalt = salt,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false
            };
            var id = await UserRepo.InsertAsync(user);
            user = await UserRepo.GetByIdAsyc(new IdentityUser
            {
                Id = id
            });
            return ServiceResult<IdentityUser>.Success(user);
        }

        public async Task<ServiceResult<string>> RegisterSiginInAsync(UserRegisterModel model)
        {
            var userResult = await RegisterUserAsync(model);
            if (!userResult.Succeed)
            {
                return ServiceResult<string>.Failed(userResult.Messages, userResult.ErrorCode);
            }
            var sessionResult = await SessionService.GetOrCreateSessionAsync(userResult.Data);
            return ServiceResult<string>.Success(sessionResult.Data.Id);
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

        public async Task<ServiceResult<BusinessSystemSignInModel>> BusinessSystemPasswordSignIn(string loginName, string password, string businessCode)
        {
            var user = await PasswordSignInAsync(loginName, password);
            if (!user.Succeed)
            {
                return ServiceResult<BusinessSystemSignInModel>.Failed(user.Messages, user.ErrorCode);
            }

            var model = new BusinessSystemSignInModel();
            var session = await SessionService.GetOrCreateSessionAsync(user.Data);
            if (!session.Succeed)
            {
                return ServiceResult<BusinessSystemSignInModel>.Failed(session.Messages, session.ErrorCode);
            }
            model.SessionId = session.Data.Id;

            var business = await BuSysRepo.FirstOrDefaultAsync(new { Code = businessCode });
            if (business != null)
            {
                model.BusinessDomainUrl = business.MainDomain;
            }

            return ServiceResult<BusinessSystemSignInModel>.Success(model);
        }

        public ServiceResult<bool> IsSignedIn(IPrincipal user)
        {
            if (!(user is ClaimsPrincipal)) return ServiceResult<bool>.Success(false);
            var principal = (ClaimsPrincipal) user;
            if(!principal.Identities.Any(i => i.AuthenticationType == ResourceConfiguration.AuthenticationScheme && i.IsAuthenticated)) return ServiceResult<bool>.Success(false);

            return ServiceResult<bool>.Success(true);
        }
    }
}
