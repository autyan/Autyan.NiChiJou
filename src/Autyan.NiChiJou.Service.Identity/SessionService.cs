﻿using System;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Component;
using Autyan.NiChiJou.Core.Config;
using Autyan.NiChiJou.Core.Extension;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Autyan.NiChiJou.Service.Identity
{
    public class SessionService : BaseService, ISessionService
    {
        private static readonly Random SeedRandom = new Random();

        private IDistributedCache Cache { get; }

        public SessionService(IDistributedCache cache)
        {
            Cache = cache;
        }

        public async Task<ServiceResult<SessionData>> GetOrCreateSessionAsync(IdentityUser user)
        {
            if(user.Id == null) return ServiceResult<SessionData>.Failed("User Id can't be null.");

            SessionData data;
            var sessionStr = await Cache.GetStringAsync($"user.sessionId.<{user.Id}>");
            if (sessionStr == null)
            {
                var sessionId = CreateSessionId();
                data = new SessionData
                {
                    Id = sessionId,
                    UserId = user.Id.Value
                };
                await Cache.SetStringAsync($"user.sessionId.<{user.Id}>", sessionId,
                    new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(ResourceConfiguration.SessionExpiration)));
                await Cache.SetStringAsync($"user.session.<{sessionId}>", JsonConvert.SerializeObject(data),
                    new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(ResourceConfiguration.SessionExpiration)));
            }
            else
            {
                data = JsonConvert.DeserializeObject<SessionData>(sessionStr);
            }

            return ServiceResult<SessionData>.Success(data);
        }

        public async Task<ServiceResult<SessionData>> GetSessionAsync(string sessionId)
        {
            var sessionStr = await Cache.GetStringAsync($"user.session.<{sessionId}>");
            if (string.IsNullOrWhiteSpace(sessionStr))
            {
                return ServiceResult<SessionData>.Failed("session not found");
            }
            return ServiceResult<SessionData>.Success(JsonConvert.DeserializeObject<SessionData>(sessionStr));
        }

        public async Task<ServiceResult<long>> GetSessionUserIdAsync(string sessionId)
        {
            var sessionStr = await Cache.GetStringAsync($"user.session.<{sessionId}>");
            if (string.IsNullOrWhiteSpace(sessionStr))
            {
                return ServiceResult<long>.Failed("session not found");
            }

            var sessionData = JsonConvert.DeserializeObject<SessionData>(sessionStr);
            return ServiceResult<long>.Success(sessionData.UserId);
        }

        private static string CreateSessionId()
        {
            var idStr = $"autyan.session.{SeedRandom.RandomString(5)}";
            return HashEncrypter.Md5Encrypt(idStr);
        }
    }
}
