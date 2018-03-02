using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.DTO.Identity;

namespace Autyan.NiChiJou.Service.Identity
{
    public interface IMembershipService
    {
        Task<ServiceResult<Membership>> FindMemberBySessionIdAsync(string sessionId);
    }
}
