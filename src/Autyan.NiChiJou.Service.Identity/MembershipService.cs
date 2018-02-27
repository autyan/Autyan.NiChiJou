using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Service.DTO.Identity;
using Microsoft.Extensions.Logging;

namespace Autyan.NiChiJou.Service.Identity
{
    public class MembershipService : BaseService, IMembershipService
    {
        private ISessionService SessionService { get; }

        public MembershipService(ILoggerFactory loggerFactory,
            ISessionService sessionService) : base(loggerFactory)
        {
            SessionService = sessionService;
        }

        public async Task<ServiceResult<Membership>> FindMemberBySessionIdAsync(string sessionId)
        {
            var session = await SessionService.GetSessionAsync(sessionId);
            if (!session.Succeed)
            {
                return FailedFrom<Membership>(session);
            }

            return Success(new Membership
            {
                MemberCode = session.Data.UserMemeberCode,
                NikeName = session.Data.UserName
            });
        }
    }
}
