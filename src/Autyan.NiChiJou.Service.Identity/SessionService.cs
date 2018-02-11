using System;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Identity;

namespace Autyan.NiChiJou.Service.Identity
{
    public class SessionService : BaseService, ISessionService
    {
        public Task<ServiceResult<SessionData>> GetOrCreateSessionAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }
    }
}
