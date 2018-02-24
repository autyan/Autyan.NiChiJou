using System.Net;
using System.Text;

namespace Autyan.NiChiJou.Core.Mvc.Authorization.ServiceTokenAuthenticationRequest
{
    public class HttpRequestAsyncState
    {
        public HttpWebRequest Request { get; }

        public string UrlEncodedContent { get; }

        public HttpResponseHandler Handler { get; }

        public HttpRequestAsyncState(HttpWebRequest request, string urlEncodedContent, HttpResponseHandler handler)
        {
            Request = request;
            UrlEncodedContent = urlEncodedContent;
            Handler = handler;
        }
    }
}
