using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Mvc.Authorization;
using Autyan.NiChiJou.Core.Mvc.Models;
using Autyan.NiChiJou.Service.Identity;

namespace Autyan.NiChiJou.IdentityServer
{
    public class ServiceTokenProvider : IServiceTokenProvider
    {
        private readonly IApplicationAuthorizationService _applicationAuthorizationService;

        public ServiceTokenProvider(IApplicationAuthorizationService applicationAuthorizationService)
        {
            _applicationAuthorizationService = applicationAuthorizationService;
        }

        public async Task<InternalServiceToken> FindServiceByNameAsync(string name)
        {
            var tokenFindResult = await _applicationAuthorizationService.FindServiceByNameAsync(name);
            if (!tokenFindResult.Succeed)
            {
                return null;
            }

            return new InternalServiceToken
            {
                ServiceName = tokenFindResult.Data.ServiceName,
                AppId = tokenFindResult.Data.AppId,
                ApiKey = tokenFindResult.Data.ApiKey
            };
        }

        public async Task<InternalServiceToken> FindServiceByAppIdAsync(string id)
        {
            var tokenFindResult = await _applicationAuthorizationService.FindServiceByAppId(id);
            if (!tokenFindResult.Succeed)
            {
                return null;
            }

            return new InternalServiceToken
            {
                ServiceName = tokenFindResult.Data.ServiceName,
                AppId = tokenFindResult.Data.AppId,
                ApiKey = tokenFindResult.Data.ApiKey
            };
        }
    }
}
