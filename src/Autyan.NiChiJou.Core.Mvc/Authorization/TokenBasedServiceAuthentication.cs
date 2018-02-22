using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Autyan.NiChiJou.Core.Mvc.Authorization
{
    public class TokenBasedServiceAuthentication : AuthenticationHandler<TokenBasedServiceAuthenticationOptions>
    {
        public TokenBasedServiceAuthentication(IOptionsMonitor<TokenBasedServiceAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Headers.ContainsKey(Options.AuthenticationSchema))
            {
                var rawAuthzHeader = Request.Headers[Options.AuthenticationSchema];
                var autherizationHeaderArray = GetAutherizationHeaderValues(rawAuthzHeader);
                if (autherizationHeaderArray != null)
                {
                    var appId = autherizationHeaderArray[0];
                    var incomingBase64Signature = autherizationHeaderArray[1];
                    var nonce = autherizationHeaderArray[2];
                    var requestTimeStamp = autherizationHeaderArray[3];

                    var isValid = IsValidRequestAsync(Request, appId, incomingBase64Signature, nonce, requestTimeStamp);

                    if (isValid.Result)
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
                        //var response =
                        //    Context.Response = new DefaultHttpResponse(Context);.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid Signature");
                        //return Task.FromResult(null);
                    }
                }
            }

            throw new System.NotImplementedException();
        }

        private static string[] GetAutherizationHeaderValues(string rawAuthzHeader)
        {

            var credArray = rawAuthzHeader.Split(':');

            return credArray.Length == 4 ? credArray : null;
        }

        private async Task<bool> IsValidRequestAsync(HttpRequest req, string appId, string incomingBase64Signature, string nonce, string requestTimeStamp)
        {
            var requestContentBase64String = "";
            var requestUri = req.GetEncodedUrl();
            var requestHttpMethod = req.Method;

            if (!_allowedAppProvider.IsAllowedApp(appId))
            {
                return false;
            }

            var sharedKey = _allowedAppProvider[appId];

            if (IsReplayRequest(nonce, requestTimeStamp))
            {
                return false;
            }

            var hash = await ComputeHashAsync();

            if (hash != null)
            {
                requestContentBase64String = Convert.ToBase64String(hash);
            }

            var data = $"{appId}{requestHttpMethod}{requestUri}{requestTimeStamp}{nonce}{requestContentBase64String}";

            var secretKeyBytes = Convert.FromBase64String(sharedKey);

            var signature = Encoding.UTF8.GetBytes(data);

            using (var hmac = new HMACSHA256(secretKeyBytes))
            {
                var signatureBytes = hmac.ComputeHash(signature);

                return (incomingBase64Signature.Equals(Convert.ToBase64String(signatureBytes), StringComparison.Ordinal));
            }

        }

        private bool IsReplayRequest(string nonce, string requestTimeStamp)
        {
            if ()
            {
                return true;
            }

            var epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            var currentTs = DateTime.UtcNow - epochStart;

            var serverTotalSeconds = Convert.ToUInt64(currentTs.TotalSeconds);
            var requestTotalSeconds = Convert.ToUInt64(requestTimeStamp);

            if ((serverTotalSeconds - requestTotalSeconds) > _requestMaxAgeInSeconds)
            {
                return true;
            }

            MemoryCache.Default.Add(nonce, requestTimeStamp, DateTimeOffset.UtcNow.AddSeconds(_requestMaxAgeInSeconds));

            return false;
        }

        private static async Task<byte[]> ComputeHashAsync(HttpContent httpContent)
        {
            using (var md5 = MD5.Create())
            {
                byte[] hash = null;
                var content = await httpContent.ReadAsByteArrayAsync();
                if (content.Length != 0)
                {
                    hash = md5.ComputeHash(content);
                }
                return hash;
            }
        }
    }
}
