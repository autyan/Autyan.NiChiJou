using System.Net;

namespace Autyan.NiChiJou.Core.Mvc.Authorization.ServiceTokenAuthenticationRequest
{
    public class HttpResponseAsyncResult
    {
        public HttpWebRequest Request { get; }

        public HttpResponseHandler Handler { get; }

        public HttpResponseAsyncResult(HttpWebRequest request, HttpResponseHandler handler)
        {
            Request = request;
            Handler = handler;
        }
    }
}
