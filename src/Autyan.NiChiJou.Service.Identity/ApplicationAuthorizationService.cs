using System.Collections.Generic;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Mvc.Extension;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Identity;
using Autyan.NiChiJou.Repository.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Autyan.NiChiJou.Service.Identity
{
    public class ApplicationAuthorizationService : BaseService, IApplicationAuthorizationService
    {
        private readonly IServiceTokenRepository _serviceTokenRepo;

        private readonly IDistributedCache _cache;

        public ApplicationAuthorizationService(ILoggerFactory loggerFactory,
            IServiceTokenRepository serviceTokenRepo,
            IDistributedCache cache) : base(loggerFactory)
        {
            _serviceTokenRepo = serviceTokenRepo;
            _cache = cache;
        }

        public async Task<ServiceResult<IEnumerable<ServiceToken>>> GetServiceAsync(ServiceTokenQuery query)
        {
            var services = await _serviceTokenRepo.QueryAsync(query);
            return Success(services);
        }

        public async Task<ServiceResult<ServiceToken>> FindServiceByNameAsync(string name)
        {
            var service = await _cache.GetDeserializedAsync<ServiceToken>($"autyan.serviceToken.name:{name}");

            if (service == null)
            {
                service = await _serviceTokenRepo.FirstOrDefaultAsync(new { ServiceName = name });
                await _cache.SetSerializedAsync($"autyan.serviceToken.name:{name}", service, new DistributedCacheEntryOptions());
            }

            if (service == null)
            {
                return Failed<ServiceToken>("service not found");
            }

            return ServiceResult<ServiceToken>.Success(service);
        }

        public async Task<ServiceResult<ServiceToken>> FindServiceByAppId(string appId)
        {
            var service = await _cache.GetDeserializedAsync<ServiceToken>($"autyan.serviceToken.appId:{appId}");

            if (service == null)
            {
                service = await _serviceTokenRepo.FirstOrDefaultAsync(new { AppId = appId });
                await _cache.SetSerializedAsync($"autyan.serviceToken.appId:{appId}", service, new DistributedCacheEntryOptions());
            }

            if (service == null)
            {
                return Failed<ServiceToken>("service not found");
            }

            return Success(service);
        }
    }
}
