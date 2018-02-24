using System.Collections.Generic;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Mvc.Extension;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Identity;
using Autyan.NiChiJou.Repository.Identity;
using Microsoft.Extensions.Caching.Distributed;

namespace Autyan.NiChiJou.Service.Identity
{
    public class ApplicationAuthorizationService : IApplicationAuthorizationService
    {
        private IServiceTokenRepository ServiceTokenRepo { get; }

        private IDistributedCache Cache { get; }

        public ApplicationAuthorizationService(IServiceTokenRepository serviceTokenRepo,
            IDistributedCache cache)
        {
            ServiceTokenRepo = serviceTokenRepo;
            Cache = cache;
        }

        public async Task<ServiceResult<IEnumerable<ServiceToken>>> GetServiceAsync(ServiceTokenQuery query)
        {
            var services = await ServiceTokenRepo.QueryAsync(query);
            return ServiceResult<IEnumerable<ServiceToken>>.Success(services);
        }

        public async Task<ServiceResult<ServiceToken>> FindServiceByNameAsync(string name)
        {
            var service = await Cache.GetDeserializedAsync<ServiceToken>($"autyan.serviceToken.name:{name}");

            if (service == null)
            {
                service = await ServiceTokenRepo.FirstOrDefaultAsync(new { ServiceName = name });
                await Cache.SetSerializedAsync($"autyan.serviceToken.name:{name}", service);
            }

            if (service == null)
            {
                return ServiceResult<ServiceToken>.Failed("service not found");
            }

            return ServiceResult<ServiceToken>.Success(service);
        }

        public async Task<ServiceResult<ServiceToken>> FindServiceByAppId(string appId)
        {
            var service = await Cache.GetDeserializedAsync<ServiceToken>($"autyan.serviceToken.appId:{appId}");

            if (service == null)
            {
                service = await ServiceTokenRepo.FirstOrDefaultAsync(new { AppId = appId });
                await Cache.SetSerializedAsync($"autyan.serviceToken.appId:{appId}", service);
            }

            if (service == null)
            {
                return ServiceResult<ServiceToken>.Failed("service not found");
            }

            return ServiceResult<ServiceToken>.Success(service);
        }
    }
}
