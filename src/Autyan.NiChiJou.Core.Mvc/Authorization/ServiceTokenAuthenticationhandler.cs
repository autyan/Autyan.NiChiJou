using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Component;
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

        private bool _isServiceTokenRequest;

        public ServiceTokenAuthenticationhandler(IOptionsMonitor<ServiceTokenAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IMemoryCache memoryCache)
            : base(options, logger, encoder, clock)
        {
            MemoryCache = memoryCache;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string appId;
            var authorizaHeader = ((FrameRequestHeaders) Request.Headers).HeaderAuthorization.ToString();
            if (!string.IsNullOrWhiteSpace(authorizaHeader) && authorizaHeader.StartsWith(Options.AuthenticationSchema))
            {
                _isServiceTokenRequest = true;
                var rawAuthzHeader = authorizaHeader.Replace(Options.AuthenticationSchema, string.Empty).Trim();
                var autherizationHeaderArray = GetAutherizationHeaderValues(rawAuthzHeader);
                if (autherizationHeaderArray != null)
                {
                    appId = autherizationHeaderArray[0];
                    var incomingBase64Signature = autherizationHeaderArray[1];
                    var nonce = autherizationHeaderArray[2];
                    var requestTimeStamp = autherizationHeaderArray[3];

                    var isValid = IsValidRequest(Request, appId, incomingBase64Signature, nonce, requestTimeStamp);

                    if (isValid)
                    {
                        var currentPrincipal = new GenericPrincipal(new GenericIdentity(appId), null);
                        Thread.CurrentPrincipal = currentPrincipal;
                        if (Context != null)
                        {
                            Context.User = currentPrincipal;
                        }
                    }
                    else
                    {
                        return Task.FromResult(AuthenticateResult.Fail("Invalid Signature"));
                    }
                }
                else
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Head"));
                }
            }
            else
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Schema"));
            }

            var claims = new List<Claim>
            {
                new Claim(nameof(appId), appId)
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

        private static string[] GetAutherizationHeaderValues(string rawAuthzHeader)
        {

            var credArray = rawAuthzHeader.Split(':');

            return credArray.Length == 4 ? credArray : null;
        }

        private bool IsValidRequest(HttpRequest req, string appId, string incomingBase64Signature, string nonce, string requestTimeStamp)
        {
            var requestContentBase64String = "";
            var requestUri = req.GetEncodedUrl();
            var requestHttpMethod = req.Method;

            if (!Options.AllowedApps.ContainsKey(appId))
            {
                return false;
            }

            var sharedKey = Options.AllowedApps[appId];

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
            if (!_isServiceTokenRequest) return Task.CompletedTask;
            return base.HandleChallengeAsync(properties);
        }
    }
}
