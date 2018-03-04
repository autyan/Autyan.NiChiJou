﻿using System;
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
        private IHttpContextAccessor HttpContextAccessor { get; }

        private AutyanCookieOptions Options { get; }

        private LoginApiManager ApiManager { get; }

        public UnifyLoginMember Member { get; set; }

        public Passport(IHttpContextAccessor httpContextAccessor,
            IOptions<AutyanCookieOptions> options,
            LoginApiManager apiManager)
        {
            HttpContextAccessor = httpContextAccessor;
            Options = options.Value;
            ApiManager = apiManager;
        }

        public async Task<bool> VerifySecurityTokenAsync(string token)
        {
            var sessionId = await ApiManager.VerifyTokenAsync(token);
            if (string.IsNullOrWhiteSpace(sessionId)) return false;
            var member = await ApiManager.GetMemberInfoAsync(sessionId);
            if (member == null) return false;
            Member = member;
            return true;
        }

        public async Task CookieLogin(IEnumerable<Claim> extraClaims)
        {
            if(Member == null) throw new ArgumentNullException(nameof(Member));
            var claims = new List<Claim>
            {
                new Claim(Options.Schema, string.Empty),
                new Claim(nameof(Member.MemberCode), Member.MemberCode),
                new Claim(ClaimTypes.Name, Member.NikeName)
            };
            if (extraClaims != null)
            {
                claims.AddRange(extraClaims);
            }

            var claimsIdentity = new ClaimsIdentity(
                claims, Options.Schema);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.UtcNow
            };

            await HttpContextAccessor.HttpContext.SignInAsync(Options.Schema,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public async Task CookieLogoutAsync()
        {
            await HttpContextAccessor.HttpContext.SignOutAsync(Options.Schema);
        }
    }
}