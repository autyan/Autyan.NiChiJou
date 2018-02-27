using System;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Mvc.Authorization.ServiceTokenAuthenticationRequest;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Autyan.NiChiJou.UnifyLogin
{
    public class LoginApiManager
    {
        private UnifyLoginOptions Options { get; }

        public LoginApiManager(IOptions<UnifyLoginOptions> options)
        {
            Options = options.Value;
        }

        public async Task<string> VerifyTokenAsync(string token)
        {
            var request = CreateRequest();
            var requestParamters = new HttpRequestParamters
            {
                QueryParamters = new
                {
                    Token = token
                }
            };

            return await request.GetStringAsync(Options.VerifyTokenAddress, requestParamters);
        }

        public async Task<UnifyLoginMember> GetMemberInfoAsync(string sessionId)
        {
            var request = CreateRequest();
            var requestParamters = new HttpRequestParamters
            {
                QueryParamters = new
                {
                    SessionId = sessionId
                }
            };
            var memberInfo = await request.GetStringAsync(Options.MemberAccessAddress, requestParamters);
            UnifyLoginMember member;
            try
            {
                member = JsonConvert.DeserializeObject<UnifyLoginMember>(memberInfo);
            }
            catch (Exception)
            {
                return null;
            }
            return member;
        }

        private RequestClient CreateRequest() => new RequestClient(Options.AppId, Options.ApiKey);
    }
}
