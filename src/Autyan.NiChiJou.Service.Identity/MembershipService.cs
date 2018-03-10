using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.DTO.Identity;
using Microsoft.Extensions.Logging;

namespace Autyan.NiChiJou.Service.Identity
{
    public class MembershipService : BaseService, IMembershipService
    {
        private readonly ISessionService _sessionService;

        public MembershipService(ILoggerFactory loggerFactory,
            ISessionService sessionService) : base(loggerFactory)
        {
            _sessionService = sessionService;
        }

        public async Task<ServiceResult<Membership>> FindMemberBySessionIdAsync(string sessionId)
        {
            var session = await _sessionService.GetSessionAsync(sessionId);
            if (!session.Succeed)
            {
                return FailedFrom<Membership>(session);
            }

            return Success(new Membership
            {
                MemberCode = session.Data.MemeberCode,
                NikeName = session.Data.UserName
            });
        }
    }
}
