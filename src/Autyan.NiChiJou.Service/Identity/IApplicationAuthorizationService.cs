using System.Collections.Generic;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Identity;

namespace Autyan.NiChiJou.Service.Identity
{
    public interface IApplicationAuthorizationService
    {
        Task<ServiceResult<IEnumerable<ServiceToken>>> GetServiceAsync(ServiceTokenQuery query);

        Task<ServiceResult<ServiceToken>> FindServiceByNameAsync(string name);

        Task<ServiceResult<ServiceToken>> FindServiceByAppId(string appId);
    }
}
