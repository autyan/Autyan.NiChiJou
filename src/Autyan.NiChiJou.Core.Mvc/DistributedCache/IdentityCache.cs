using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Options;

namespace Autyan.NiChiJou.Core.Mvc.DistributedCache
{
    public class IdentityCache : RedisCache, IIdentityCache
    {
        public IdentityCache(IOptions<IdentityCacheOptions> optionsAccessor) : base(optionsAccessor)
        {

        }
    }
}
