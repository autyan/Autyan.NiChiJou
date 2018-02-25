using System;
using System.Threading.Tasks;
using Autyan.NiChiJou.BusinessModel.Identity;
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

        public async Task<string> VerifyTokenAsync(string token, string accessUrl)
        {
            var request = CreateRequest();
            var requestParamters = new HttpRequestParamters
            {
                BodyParamters = new TokenVerificationViewMoodel
                {
                    Token = token,
                    ReturnUrl = accessUrl
                }
            };

            return await request.StartRequestAsync(Options.VerifyTokenAddress, RequestClient.HttpMethodPost, requestParamters);
        }

        public async Task<UnifyLoginMember> GetMemberInfoAsync(string memberCode)
        {
            var request = CreateRequest();
            var requestParamters = new HttpRequestParamters
            {
                BodyParamters = new { memberCode }
            };
            var memberInfo = await request.StartRequestAsync(Options.MemberAccessAddress, RequestClient.HttpMethodPost, requestParamters);
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
