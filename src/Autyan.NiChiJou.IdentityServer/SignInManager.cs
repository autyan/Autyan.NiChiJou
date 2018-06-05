using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Component;
using Autyan.NiChiJou.Core.Mvc.Authorization;
using Autyan.NiChiJou.Core.Options;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.DTO.Identity;
using Autyan.NiChiJou.Service.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Autyan.NiChiJou.IdentityServer
{
    public class SignInManager
    {
        private ISignInService SignInService { get; }

        private ISessionService SessionService { get; }

        private IDistributedCache Cache { get; }

        public Controller Controller { get; set; }

        private AutyanCookieOptions Options { get; }

        private IHttpContextAccessor HttpContextAccessor { get; }

        public SignInManager(ISignInService signInService,
            ISessionService sessionService,
            IDistributedCache cache,
            IOptions<AutyanCookieOptions> options,
            IHttpContextAccessor httpContextAccessor)
        {
            SignInService = signInService;
            SessionService = sessionService;
            Cache = cache;
            Options = options.Value;
            HttpContextAccessor = httpContextAccessor;
        }

        public string CurrentSessinId => GetCurrentUserSessionId();

        public async Task<ServiceResult<SignInInfomation>> RegisterUserAsync(UserRegistration model)
        {
            var registerResult = await SignInService.RegisterUserAsync(model);
            if (!registerResult.Succeed || registerResult.Data.Id == null)
            {
                return ServiceResult<SignInInfomation>.FailedFrom(registerResult);
            }

            return ServiceResult<SignInInfomation>.Success(new SignInInfomation
            {
                UserId = registerResult.Data.Id.Value
            });
        }

        public async Task<ServiceResult<SignInInfomation>> PasswordSignInAsync(string loginName, string password)
        {
            var signInResult = await SignInService.PasswordSignInAsync(loginName, password);
            if (!signInResult.Succeed) return ServiceResult<SignInInfomation>.FailedFrom(signInResult);

            var sessionResult = await SessionService.CreateSessionAsync(signInResult.Data);
            if (!sessionResult.Succeed) return ServiceResult<SignInInfomation>.FailedFrom(sessionResult);
            await CookieSignInAsync(sessionResult.Data.Id);

            return ServiceResult<SignInInfomation>.Success(new SignInInfomation
            {
                SessionId = sessionResult.Data.Id
            });
        }

        public bool IsSignedIn()
        {
            var user = Controller.HttpContext.User;
            if (user == null) return false;
            var principal = user;
            return principal.Identities.Any(i => i.AuthenticationType == Options.Schema && i.IsAuthenticated);
        }

        private string GetCurrentUserSessionId()
        {
            var claim = HttpContextAccessor.HttpContext.User.FindFirst("SessionId");
            if (claim == null) return string.Empty;
            return claim.Value;
        }

        public async Task CookieSignInAsync(string sessionId)
        {
            var sessionData = await SessionService.GetSessionAsync(sessionId);
            var claims = new List<Claim>
            {
                new Claim(Options.Schema, string.Empty),
                new Claim("SessionId", sessionId),
                new Claim(ClaimTypes.Name, sessionData.Data.UserName)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, Options.Schema);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.UtcNow
            };

            await Controller.HttpContext.SignInAsync(
                Options.Schema,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public async Task<ServiceResult<string>> CreateLoginVerificationTokenAsync(string sessionId = null)
        {
            var randomValue = Guid.NewGuid().ToString();
            var token = HashEncrypter.Md5EncryptToBase64(randomValue);
            if (sessionId == null)
            {
                sessionId = CurrentSessinId;
            }

            await Cache.SetStringAsync($"login.verificationtoken.{token}", sessionId, new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            });

            return ServiceResult<string>.Success(token);
        }

        public async Task<ServiceResult<string>> GetSessionIdByVerificationTokenAsync(string token)
        {
            var sessionId = await Cache.GetStringAsync($"login.verificationtoken.{token}");
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return ServiceResult<string>.Failed("verification token not found");
            }

            var sessionResult = await SessionService.GetSessionAsync(sessionId);
            if (!sessionResult.Succeed)
            {
                return ServiceResult<string>.Failed("session not found");
            }
            await Cache.RemoveAsync($"login.verificationtoken.{token}");

            return ServiceResult<string>.Success(sessionResult.Data.Id);
        }

        public async Task<IActionResult> SignInRedirectAsync(string returnUrl, string subSystem, string sessionId = null)
        {
            if (string.IsNullOrWhiteSpace(subSystem))
            {
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    return Controller.RedirectToAction("Index", "Home");
                }

                return Controller.Redirect(returnUrl);
            }

            var token = await CreateLoginVerificationTokenAsync(sessionId);
            return Controller.Redirect(
                $"{returnUrl}?token={token.Data}");
        }
    }
}
