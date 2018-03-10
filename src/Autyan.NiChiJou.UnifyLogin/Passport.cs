using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Autyan.NiChiJou.UnifyLogin
{
    public class Passport
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AutyanCookieOptions _options;

        private readonly LoginApiManager _apiManager;

        public UnifyLoginMember Member { get; private set; }

        public Passport(IHttpContextAccessor httpContextAccessor,
            IOptions<AutyanCookieOptions> options,
            LoginApiManager apiManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _options = options.Value;
            _apiManager = apiManager;
        }

        public async Task<bool> VerifySecurityTokenAsync(string token)
        {
            var sessionId = await _apiManager.VerifyTokenAsync(token);
            if (string.IsNullOrWhiteSpace(sessionId)) return false;
            var member = await _apiManager.GetMemberInfoAsync(sessionId);
            if (member == null) return false;
            Member = member;
            return true;
        }

        public async Task CookieLogin(IEnumerable<Claim> extraClaims)
        {
            if(Member == null) throw new ArgumentNullException(nameof(Member));
            var claims = new List<Claim>
            {
                new Claim(_options.Schema, string.Empty),
                new Claim(nameof(Member.MemberCode), Member.MemberCode),
                new Claim(ClaimTypes.Name, Member.NikeName)
            };
            if (extraClaims != null)
            {
                claims.AddRange(extraClaims);
            }

            var claimsIdentity = new ClaimsIdentity(
                claims, _options.Schema);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.UtcNow
            };

            await _httpContextAccessor.HttpContext.SignInAsync(_options.Schema,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public async Task CookieLogoutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(_options.Schema);
        }
    }
}
