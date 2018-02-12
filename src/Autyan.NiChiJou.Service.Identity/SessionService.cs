using System;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Component;
using Autyan.NiChiJou.Core.Config;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Autyan.NiChiJou.Service.Identity
{
    public class SessionService : BaseService, ISessionService
    {
        private IDistributedCache Cache { get; }

        public SessionService(IDistributedCache cache)
        {
            Cache = cache;
        }

        public async Task<ServiceResult<SessionData>> GetOrCreateSessionAsync(IdentityUser user)
        {
            if(user.Id == null) return ServiceResult<SessionData>.Failed("User Id can't be null.");

            SessionData data;
            var sessionStr = await Cache.GetStringAsync($"user.session.<{user.Id}>");
            if (sessionStr == null)
            {
                var sessionId = await CreateSessionIdAsync();
                data = new SessionData
                {
                    Id = sessionId,
                    UserId = user.Id.Value
                };
                await Cache.SetStringAsync($"user.session.<{user.Id}>", JsonConvert.SerializeObject(data),
                    new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(ResourceConfiguration.SessionExpiration)));
            }
            else
            {
                data = JsonConvert.DeserializeObject<SessionData>(sessionStr);
            }

            return ServiceResult<SessionData>.Success(data);
        }

        private async Task<string> CreateSessionIdAsync()
        {
            ulong currentId = 0;
            var current = await Cache.GetAsync("sessionSequence.current");
            if (current != null)
            {
                currentId = BitConverter.ToUInt64(current, 0);
            }

            var next = currentId + 1;
            await Cache.SetAsync("sessionSequence.current", BitConverter.GetBytes(next));
            var idStr = $"autyan.session.{next}";
            return HashEncrypter.Md5Encrypt(idStr);
        }
    }
}
