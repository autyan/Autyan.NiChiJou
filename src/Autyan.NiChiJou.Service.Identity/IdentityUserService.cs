using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Identity;
using Autyan.NiChiJou.Repository.Identity;
using Autyan.NiChiJou.Service.Identity.ServiceStatusCode;
using Microsoft.Extensions.Logging;

namespace Autyan.NiChiJou.Service.Identity
{
    public class IdentityUserService : BaseService, IIdentityUserService
    {
        private readonly IIdentityUserRepository _userRepo;

        public IdentityUserService(IIdentityUserRepository userRepo,
            ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _userRepo = userRepo;
        }

        public async Task<ServiceResult<IdentityUser>> GetUserByIdAsync(long id)
        {
            var user = await _userRepo.GetByIdAsync(new IdentityUser {Id = id});
            if (user == null)
            {
                return Failed<IdentityUser>(IdentityStatus.UserNotFound);
            }

            return Success(user);
        }

    }
}
