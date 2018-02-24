using System;

namespace Autyan.NiChiJou.Core.Mvc.Authorization.ServiceTokenAuthenticationRequest
{
    public class HttpResponseHandler
    {
        public Action<HttpRerquestEventArgs> OnResponse { get; set; }

        public Action<HttpRerquestEventArgs> OnError { get; set; }

        public void Response(string response)
        {
            OnResponse?.Invoke(new HttpRerquestEventArgs
            {
                Response = response
            });
        }

        public void Error(Exception exception)
        {
            OnError?.Invoke(new HttpRerquestEventArgs
            {
                Exception = exception
            });
        }
    }
}
