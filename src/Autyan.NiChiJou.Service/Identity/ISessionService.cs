﻿using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.Model.Identity;

namespace Autyan.NiChiJou.Service.Identity
{
    public interface ISessionService
    {
        Task<ServiceResult<SessionData>> CreateSessionAsync(IdentityUser user);

        Task<ServiceResult<SessionData>> GetSessionAsync(string sessionId);

        Task<ServiceResult<long>> GetSessionUserIdAsync(string sessionId);

        Task RemoveSessionAsync(string sessionId);
    }
}
