using System.Collections.Generic;

namespace Autyan.NiChiJou.Core.Mvc.Authorization.ServiceTokenAuthenticationRequest
{
    public class HttpRequestParamters
    {
        public Dictionary<string, string> HeaderStrings { get; }

        public object BodyParamters { get; set; }

        public HttpRequestParamters()
        {
            HeaderStrings = new Dictionary<string, string>();
        }

        public void AddHeader(string key, string value)
        {
            HeaderStrings[key] = value;
        }
    }
}
