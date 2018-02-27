using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Autyan.NiChiJou.Core.Extension;

namespace Autyan.NiChiJou.Core.Mvc.Authorization.ServiceTokenAuthenticationRequest
{
    public class RequestClient
    {
        private readonly string _appId;

        private readonly string _apiKey;

        public RequestClient(string appId, string apiKey)
        {
            _appId = appId;
            _apiKey = apiKey;
        }

        private void AddAuthenticationHead(Uri apiUrl, string method, HttpRequestParamters paramter)
        {
            var requestUri = HttpUtility.UrlEncode(apiUrl.ToString().ToLower());
            var requestHttpMethod = method;
            var requestTimeStamp = Convert.ToUInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).ToString();
            var nonce = Guid.NewGuid().ToString("N");
            var requestBody = string.Empty;
            if (paramter.PostParamters != null)
            {
                requestBody = paramter.PostParamters.ReadAsStringAsync().Result;
            }

            var requestContentBase64String = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(requestBody)));
            var s = _appId + requestHttpMethod + requestUri + requestTimeStamp + nonce + requestContentBase64String;
            var key = Convert.FromBase64String(_apiKey);
            var bytes = Encoding.UTF8.GetBytes(s);
            using (var hmacshA256 = new HMACSHA256(key))
            {
                var base64String = Convert.ToBase64String(hmacshA256.ComputeHash(bytes));
                paramter.AddHeader(nameof(Authorization), $"autyan.serviceToken {_appId}:{base64String}:{nonce}:{requestTimeStamp}");
            }
        }

        public async Task<string> GetStringAsync(string apiUrl, HttpRequestParamters paramters)
        {
            using (var client = new HttpClient())
            {
                var requestUrl = CombineQueryToUri(apiUrl, paramters.QueryParamters);
                InitialRequest(client);
                AddAuthenticationHead(requestUrl, "GET", paramters);
                foreach (var header in paramters.HeaderStrings)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                return await client.GetStringAsync(requestUrl);
            }
        }

        private static void InitialRequest(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static Uri CombineQueryToUri(string requestUrl, object queryParamters)
        {
            if (queryParamters == null) return new Uri(requestUrl);
            var queryString = string.Join("$",
                queryParamters.ToKeyValue().Select(item => $"{item.Key}={HttpUtility.UrlEncode(item.Value)}"));
            return new Uri($"{requestUrl}?{queryString}");
        }
    }
}
