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
    public class UnifyLogin
    {
        private IHttpContextAccessor HttpContextAccessor { get; }

        private AutyanCookieOptions Options { get; }

        private LoginApiManager ApiManager { get; }

        public UnifyLogin(IHttpContextAccessor httpContextAccessor,
            IOptions<AutyanCookieOptions> options,
            LoginApiManager apiManager)
        {
            HttpContextAccessor = httpContextAccessor;
            Options = options.Value;
            ApiManager = apiManager;
        }

        public async Task HandlerLoginAsync(string token, string accessUrl)
        {
            var memberCode = await ApiManager.VerifyTokenAsync(token, accessUrl);
            var member = await ApiManager.GetMemberInfoAsync(memberCode);
            var claims = new List<Claim>
            {
                new Claim(Options.Scheme, string.Empty),
                new Claim(nameof(member.MemberCode), member.MemberCode),
                new Claim(ClaimTypes.Name, member.NikeName)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, Options.Scheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.UtcNow
            };

            await HttpContextAccessor.HttpContext.SignInAsync(Options.Scheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}
