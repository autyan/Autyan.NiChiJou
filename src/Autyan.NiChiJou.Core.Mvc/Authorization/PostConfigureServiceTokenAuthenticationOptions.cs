using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Autyan.NiChiJou.Core.Mvc.Authorization
{
    class PostConfigureServiceTokenAuthenticationOptions : IPostConfigureOptions<ServiceTokenAuthenticationOptions>
    {
        public void PostConfigure(string name, ServiceTokenAuthenticationOptions options)
        {
            if (options.RequestMaxAgeSeconds == 0)
            {
                options.RequestMaxAgeSeconds = 300;
            }

            if (string.IsNullOrWhiteSpace(options.AuthenticationSchema))
            {
                options.AuthenticationSchema = "autyan.serviceToken";
            }
        }
    }
}
