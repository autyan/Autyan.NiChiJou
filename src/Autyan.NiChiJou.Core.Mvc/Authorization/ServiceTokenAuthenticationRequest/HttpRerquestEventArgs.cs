using System;

namespace Autyan.NiChiJou.Core.Mvc.Authorization.ServiceTokenAuthenticationRequest
{
    public class HttpRerquestEventArgs
    {
        public string Response { get; set; }

        public string Error { get; set; }

        public Exception Exception { get; set; }
    }
}
