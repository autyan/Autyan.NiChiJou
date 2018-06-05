using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Context;
using Autyan.NiChiJou.Core.Mvc.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace Autyan.NiChiJou.Core.Mvc.Context
{
    public class SessionIdentityContext<T> : IIdentityContext<T> where T : class, new()
    {
        private IDistributedCache Cache { get; }

        private HttpContext Context { get; }

        private T _identity;

        private string Key {get;}

        public T Identity => _identity ??
                             (_identity =
                                 Cache.GetDeserializedAsync<T>($"SessionIdContext.<{typeof(T).FullName}>.<{Key}>").Result ??
                                 new T());

        public ClaimsPrincipal User { get; }

        public SessionIdentityContext(IDistributedCache cache,
            IHttpContextAccessor httpContextAccessor)
        {
            Cache = cache;
            Context = httpContextAccessor.HttpContext;
            User = Context?.User;
            Key = User?.Claims.FirstOrDefault(c => c.Type == "MemberCode")?.Value;
        }

        public async Task SetIdentityAsync(string key,T identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            await Cache.SetSerializedAsync($"SessionIdContext.<{typeof(T).FullName}>.<{key}>", identity, new DistributedCacheEntryOptions());
            _identity = identity;
        }
    }
}
