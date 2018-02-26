using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Identity;
using Autyan.NiChiJou.Repository.Identity;
using Microsoft.Extensions.Logging;

namespace Autyan.NiChiJou.Service.Identity
{
    public class IdentityUserService : BaseService, IIdentityUserService
    {
        protected static IIdentityUserRepository UserRepo { get; private set; }

        public IdentityUserService(IIdentityUserRepository userRepo,
            ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            UserRepo = userRepo;
        }

        public async Task<ServiceResult<IdentityUser>> GetUserByIdAsync(long id)
        {
            var user = await UserRepo.GetByIdAsync(new IdentityUser {Id = id});
            if (user == null)
            {
                return ServiceResult<IdentityUser>.Failed("user not found", (int) IdentityStatus.UserNotFound);
            }

            return ServiceResult<IdentityUser>.Success(user);
        }

    }
}
