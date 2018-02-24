using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Autyan.NiChiJou.Core.Extension;

namespace Autyan.NiChiJou.Core.Mvc.Authorization.ServiceTokenAuthenticationRequest
{
    public class RequestClient
    {
        public static string HttpMethodPost = "POST";

        public static string HttpMethodGet = "GET";

        private readonly string _appId;

        private readonly string _apiKey;

        public RequestClient(string appId, string apiKey)
        {
            _appId = appId;
            _apiKey = apiKey;
        }

        public void StartRequest(string api, string method, HttpRequestParamters paramter, HttpResponseHandler handler)
        {
            var request = (HttpWebRequest)WebRequest.Create(api);
            request.Method = method;
            request.Accept = "application/json";
            request.ContentType = "application/x-www-form-urlencoded";
            AddAuthenticationHead(new Uri(api), method, paramter);
            foreach (var headerString in paramter.HeaderStrings)
                request.Headers[headerString.Key] = headerString.Value;
            if (paramter.BodyParamters != null)
            {
                var urlEncodedContent = paramter.BodyParamters is string ? paramter.BodyParamters.ToString()
                    : new FormUrlEncodedContent(paramter.BodyParamters.ToKeyValue()).ReadAsStringAsync().Result;
                request.BeginGetRequestStream(PostCallBack, new HttpRequestAsyncState(request, urlEncodedContent, handler));
            }
            else
                request.BeginGetResponse(ReadCallBack, new HttpResponseAsyncResult(request, handler));
        }

        public Task<string> StartRequestAsync(string api, string method, HttpRequestParamters paramter, bool needAuthtication = false)
        {
            var request = (HttpWebRequest)WebRequest.Create(api);
            request.Method = method;
            request.Accept = "application/json";
            request.ContentType = "application/x-www-form-urlencoded";
            if (needAuthtication)
            {
                AddAuthenticationHead(new Uri(api), method, paramter);
            }
            foreach (var headerString in paramter.HeaderStrings)
                request.Headers.Add(headerString.Key, headerString.Value);
            if (paramter.BodyParamters != null)
            {
                var s = paramter.BodyParamters is string ? paramter.BodyParamters.ToString()
                    : new FormUrlEncodedContent(paramter.BodyParamters.ToKeyValue()).ReadAsStringAsync().Result;
                var requestStream = request.GetRequestStream();
                var bytes = Encoding.UTF8.GetBytes(s);
                requestStream.Write(bytes, 0, bytes.Length);
            }
            return Task.Factory.FromAsync(request.BeginGetResponse, asyncResult => request.EndGetResponse(asyncResult), null).ContinueWith(t => ReadStreamFromResponse(t.Result));
        }

        private static void PostCallBack(IAsyncResult asynchronousResult)
        {
            var asyncState = (HttpRequestAsyncState)asynchronousResult.AsyncState;
            try
            {
                var requestStream = asyncState.Request.EndGetRequestStream(asynchronousResult);
                var bytes = Encoding.UTF8.GetBytes(asyncState.UrlEncodedContent);
                requestStream.Write(bytes, 0, bytes.Length);
                asyncState.Request.BeginGetResponse(ReadCallBack, new HttpResponseAsyncResult(asyncState.Request, asyncState.Handler));
            }
            catch (Exception ex)
            {
                asyncState.Handler.Error(ex);
            }
        }

        private static void ReadCallBack(IAsyncResult asynchronousResult)
        {
            var asyncState = (HttpResponseAsyncResult)asynchronousResult.AsyncState;
            try
            {
                var responseStream = asyncState.Request.EndGetResponse(asynchronousResult).GetResponseStream();
                if (responseStream == null)
                {
                    asyncState.Handler.Response(string.Empty);
                }
                else
                {
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        var end = streamReader.ReadToEnd();
                        asyncState.Handler.Response(end);
                    }
                }
            }
            catch (Exception ex)
            {
                asyncState.Handler.Error(ex);
            }
        }

        private static string ReadStreamFromResponse(WebResponse response)
        {
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream == null)
                    return string.Empty;
                using (var streamReader = new StreamReader(responseStream))
                    return streamReader.ReadToEnd();
            }
        }

        private void AddAuthenticationHead(Uri apiUrl, string method, HttpRequestParamters paramter)
        {
            var requestContentBase64String = string.Empty;
            var requestUri = HttpUtility.UrlEncode(apiUrl.ToString().ToLower());
            var requestHttpMethod = method;
            var requestTimeStamp = Convert.ToUInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).ToString();
            var nonce = Guid.NewGuid().ToString("N");
            if (paramter.BodyParamters != null)
            {
                requestContentBase64String = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(paramter.BodyParamters is string
                    ? paramter.BodyParamters.ToString()
                    : new FormUrlEncodedContent(paramter.BodyParamters.ToKeyValue()).ReadAsStringAsync().Result)));
            }
            var s = _appId + requestHttpMethod + requestUri + requestTimeStamp + nonce + requestContentBase64String;
            var key = Convert.FromBase64String(_apiKey);
            var bytes = Encoding.UTF8.GetBytes(s);
            using (var hmacshA256 = new HMACSHA256(key))
            {
                var base64String = Convert.ToBase64String(hmacshA256.ComputeHash(bytes));
                paramter.AddHeader(nameof(Authorization), $"cpx {_appId}:{base64String}:{nonce}:{requestTimeStamp}");
            }
        }
    }
}
