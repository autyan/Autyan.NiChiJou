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

        private readonly IDistributedCache _cache;

        private readonly IConfiguration _configuration;

        public SessionService(IDistributedCache cache,
            IConfiguration configuration,
            ILoggerFactory loggerFactory): base(loggerFactory)
        {
            _cache = cache;
            _configuration = configuration;
        }

        public async Task<ServiceResult<SessionData>> CreateSessionAsync(IdentityUser user)
        {
            if(user.Id == null) return Failed<SessionData>(SessionServiceStatus.UserIdIsNull);

            var sessionId = CreateSessionId();
            var data = new SessionData
            {
                Id = sessionId,
                UserId = user.Id.Value,
                UserName = user.NickName,
                MemeberCode = user.MemberCode
            };
            await _cache.SetStringAsync($"user.session.<{sessionId}>", JsonConvert.SerializeObject(data),
                new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(double.Parse(_configuration["Session:Expiration"]))));

            return Success(data);
        }

        public async Task<ServiceResult<SessionData>> GetSessionAsync(string sessionId)
        {
            var sessionStr = await _cache.GetStringAsync($"user.session.<{sessionId}>");
            if (string.IsNullOrWhiteSpace(sessionStr))
            {
                return Failed<SessionData>(SessionServiceStatus.SessionNotFound);
            }
            return Success(JsonConvert.DeserializeObject<SessionData>(sessionStr));
        }

        public async Task<ServiceResult<long>> GetSessionUserIdAsync(string sessionId)
        {
            var sessionStr = await _cache.GetStringAsync($"user.session.<{sessionId}>");
            if (string.IsNullOrWhiteSpace(sessionStr))
            {
                return Failed<long>(SessionServiceStatus.SessionNotFound);
            }

            var sessionData = JsonConvert.DeserializeObject<SessionData>(sessionStr);
            return Success(sessionData.UserId);
        }

        public async Task RemoveSessionAsync(string sessionId)
        {
            await _cache.RemoveAsync($"user.session.<{sessionId}>");
        }

        private static string CreateSessionId()
        {
            var idStr = $"autyan.session.{SeedRandom.RandomString(5)}";
            return HashEncrypter.Md5EncryptToBase64(idStr);
        }
    }
}
