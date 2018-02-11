using Autyan.NiChiJou.Model.Identity;
using Autyan.NiChiJou.Repository.Identity;

namespace Autyan.NiChiJiu.Repository.Redis.Identity
{
    public class SessionDataRepository : RedisRepository<SessionData>, ISessionDataRepository
    {
    }
}
