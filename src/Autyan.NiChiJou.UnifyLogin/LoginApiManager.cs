using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Config;
using Autyan.NiChiJou.Core.Mvc.Authorization.ServiceTokenAuthenticationRequest;

namespace Autyan.NiChiJou.UnifyLogin
{
    public static class LoginApiManager
    {
        private static string UnifyLoginServer => ResourceConfiguration.UnifyLoginServer;

        private static string AppId => ResourceConfiguration.ServiceTokenAppId;

        private static string ApiKey => ResourceConfiguration.ServiceTokenApiKey;

        public static async Task GetMemberInfoAsync(string memberCode)
        {
            var request = CreateRequest();
            await request.StartRequestAsync(UnifyLoginServer, RequestClient.HttpMethodPost, new HttpRequestParamters());
        }

        private static RequestClient CreateRequest() => new RequestClient(AppId, ApiKey);
    }
}
