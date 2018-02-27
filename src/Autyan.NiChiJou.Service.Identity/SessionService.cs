using System;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Component;
using Autyan.NiChiJou.Core.Extension;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Identity;
using Autyan.NiChiJou.Service.Identity.ServiceStatusCode;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Autyan.NiChiJou.Service.Identity
{
    public class SessionService : BaseService, ISessionService
    {
        private static readonly Random SeedRandom = new Random();

        private IDistributedCache Cache { get; }

        private IConfiguration Configuration { get; }

        public SessionService(IDistributedCache cache,
            IConfiguration configuration,
            ILoggerFactory loggerFactory): base(loggerFactory)
        {
            Cache = cache;
            Configuration = configuration;
        }

        public async Task<ServiceResult<SessionData>> CreateSessionAsync(IdentityUser user)
        {
            if(user.Id == null) return ServiceResult<SessionData>.Failed(SessionServiceStatus.UserIdIsNull);

            var sessionId = CreateSessionId();
            var data = new SessionData
            {
                Id = sessionId,
                UserId = user.Id.Value,
                UserName = user.NickName,
                UserMemeberCode = user.UserMemberCode
            };
            await Cache.SetStringAsync($"user.session.<{sessionId}>", JsonConvert.SerializeObject(data),
                new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(double.Parse(Configuration["Session:Expiration"]))));

            return ServiceResult<SessionData>.Success(data);
        }

        public async Task<ServiceResult<SessionData>> GetSessionAsync(string sessionId)
        {
            var sessionStr = await Cache.GetStringAsync($"user.session.<{sessionId}>");
            if (string.IsNullOrWhiteSpace(sessionStr))
            {
                return ServiceResult<SessionData>.Failed(SessionServiceStatus.SessionNotFound);
            }
            return ServiceResult<SessionData>.Success(JsonConvert.DeserializeObject<SessionData>(sessionStr));
        }

        public async Task<ServiceResult<long>> GetSessionUserIdAsync(string sessionId)
        {
            var sessionStr = await Cache.GetStringAsync($"user.session.<{sessionId}>");
            if (string.IsNullOrWhiteSpace(sessionStr))
            {
                return ServiceResult<long>.Failed(SessionServiceStatus.SessionNotFound);
            }

            var sessionData = JsonConvert.DeserializeObject<SessionData>(sessionStr);
            return ServiceResult<long>.Success(sessionData.UserId);
        }

        public async Task RemoveSessionAsync(string sessionId)
        {
            await Cache.RemoveAsync($"user.session.<{sessionId}>");
        }

        private static string CreateSessionId()
        {
            var idStr = $"autyan.session.{SeedRandom.RandomString(5)}";
            return HashEncrypter.Md5EncryptToBase64(idStr);
        }
    }
}
