using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Component;
using Autyan.NiChiJou.Model.Identity;
using Autyan.NiChiJou.Service.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Autyan.NiChiJou.Core.Mvc.Authorization
{
    public class ServiceTokenAuthenticationhandler : AuthenticationHandler<ServiceTokenAuthenticationOptions>, IAuthorizationRequirement
    {
        private IMemoryCache MemoryCache { get; }

        private IApplicationAuthorizationService ApplicationAuthorizationService { get; }

        private bool IsServiceTokenRequest { get; set; }

        public ServiceTokenAuthenticationhandler(IOptionsMonitor<ServiceTokenAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IMemoryCache memoryCache,
            IApplicationAuthorizationService applicationAuthorizationService)
            : base(options, logger, encoder, clock)
        {
            MemoryCache = memoryCache;
            ApplicationAuthorizationService = applicationAuthorizationService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authorizaHeader = ((FrameRequestHeaders) Request.Headers).HeaderAuthorization.ToString();
            if (!string.IsNullOrWhiteSpace(authorizaHeader) && authorizaHeader.StartsWith(Options.AuthenticationSchema))
            {
                IsServiceTokenRequest = true;
                var rawAuthzHeader = authorizaHeader.Replace(Options.AuthenticationSchema, string.Empty).Trim();
                var autherizationHeaderArray = GetAutherizationHeaderValues(rawAuthzHeader);
                if (autherizationHeaderArray != null)
                {
                    var appId = autherizationHeaderArray[0];
                    var incomingBase64Signature = autherizationHeaderArray[1];
                    var nonce = autherizationHeaderArray[2];
                    var requestTimeStamp = autherizationHeaderArray[3];

                    var isValid = IsValidRequest(Request, appId, incomingBase64Signature, nonce, requestTimeStamp, out var token);

                    if (isValid)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(Options.AuthenticationSchema, string.Empty),
                            new Claim(nameof(token.ServiceName), token.ServiceName),
                            new Claim(nameof(token.AppId), appId)
                        };
                        var claimsIdentity = new ClaimsIdentity(claims, Options.AuthenticationSchema);
                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = false,
                            IsPersistent = false,
                            IssuedUtc = DateTimeOffset.UtcNow
                        };
                        return Task.FromResult(
                            AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), authProperties,
                                Options.AuthenticationSchema)));
                    }

                    return Task.FromResult(AuthenticateResult.Fail("Invalid Signature"));
                }

                return Task.FromResult(AuthenticateResult.Fail("Invalid Head"));
            }

            return Task.FromResult(AuthenticateResult.Fail("Invalid Schema"));
        }

        private static string[] GetAutherizationHeaderValues(string rawAuthzHeader)
        {

            var credArray = rawAuthzHeader.Split(':');

            return credArray.Length == 4 ? credArray : null;
        }

        private bool IsValidRequest(HttpRequest req, string appId, string incomingBase64Signature, string nonce, string requestTimeStamp, out ServiceToken tokenInfo)
        {
            tokenInfo = null;
            var requestContentBase64String = "";
            var requestUri = req.GetEncodedUrl();
            var requestHttpMethod = req.Method;

            var app = ApplicationAuthorizationService.FindServiceByAppId(appId).Result;
            if (!app.Succeed)
            {
                return false;
            }

            tokenInfo = app.Data;
            var sharedKey = tokenInfo.ApiKey;

            if (IsReplayRequest(nonce, requestTimeStamp))
            {
                return false;
            }

            string requestBody;
            using (var reader = new StreamReader(Request.Body))
            {
                requestBody = reader.ReadToEnd();
            }
            var hash = HashEncrypter.Md5Encrypt(requestBody);

            if (hash != null)
            {
                requestContentBase64String = Convert.ToBase64String(hash);
            }

            var data = appId + requestHttpMethod + requestUri + requestTimeStamp + nonce + requestContentBase64String;

            var secretKeyBytes = Convert.FromBase64String(sharedKey);

            var signature = Encoding.UTF8.GetBytes(data);

            return (incomingBase64Signature.Equals(HashEncrypter.Hmacsha256EncryptToBase64(secretKeyBytes, signature), StringComparison.Ordinal));
        }

        private bool IsReplayRequest(string nonce, string requestTimeStamp)
        {
            if (MemoryCache.TryGetValue(nonce, out object _))
            {
                return true;
            }

            var epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            var currentTs = DateTime.UtcNow - epochStart;

            var serverTotalSeconds = Convert.ToUInt64(currentTs.TotalSeconds);
            var requestTotalSeconds = Convert.ToUInt64(requestTimeStamp);

            if ((serverTotalSeconds - requestTotalSeconds) > Options.RequestMaxAgeSeconds)
            {
                return true;
            }

            MemoryCache.Set(nonce, requestTimeStamp, DateTimeOffset.UtcNow.AddSeconds(Options.RequestMaxAgeSeconds));

            return false;
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            if (!IsServiceTokenRequest) return Task.CompletedTask;
            return base.HandleChallengeAsync(properties);
        }
    }
}
