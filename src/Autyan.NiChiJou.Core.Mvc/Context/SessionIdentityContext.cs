using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Component;
using Autyan.NiChiJou.Core.Context;
using Autyan.NiChiJou.Core.Mvc.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace Autyan.NiChiJou.Core.Mvc.Context
{
    public class SessionIdentityContext<T> : IIdentityContext<T> where T : class, new()
    {
        private IDistributedCache Cache { get; }

        private IHttpContextAccessor HttpContextAccessor { get; }

        private T _identity;

        private string Key {get;}

        public T Ientity => _identity ??
                            (_identity = Cache.GetDeserializedAsync<T>($"SessionIdContext.<{typeof(T).FullName}>.<{Key}>").Result ?? new T());

        public SessionIdentityContext(IDistributedCache cache,
            IHttpContextAccessor httpContextAccessor)
        {
            Cache = cache;
            HttpContextAccessor = httpContextAccessor;
            Key = HttpContextAccessor.HttpContext.Request.Cookies["SessionIdContext"];
        }

        public async Task SetIdentityAsync(string key,T identity)
        {
            var keyMd5 = HashEncrypter.Md5EncryptToBase64(key);
            HttpContextAccessor.HttpContext.Response.Cookies.Append("SessionIdContext", keyMd5);
            await Cache.SetSerializedAsync($"SessionIdContext.<{typeof(T).FullName}>.<{keyMd5}>", identity, new DistributedCacheEntryOptions());
        }
    }
}
