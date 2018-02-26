using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Autyan.NiChiJou.Core.Mvc.Extension
{
    public static class DistributedCacheExtensions
    {
        public static async Task<T> GetDeserializedAsync<T>(this IDistributedCache cache, string key)
        {
            var strValue = await cache.GetStringAsync(key);
            if (string.IsNullOrWhiteSpace(strValue))
            {
                return default(T);
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(strValue);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static async Task SetSerializedAsync(this IDistributedCache cache, string key, object target, DistributedCacheEntryOptions options)
        {
            var strValue = JsonConvert.SerializeObject(target);
            await cache.SetStringAsync(key, strValue, options);
        }
    }
}
