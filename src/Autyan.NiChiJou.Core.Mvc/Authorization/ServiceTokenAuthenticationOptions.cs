using Microsoft.AspNetCore.Authentication;

namespace Autyan.NiChiJou.Core.Mvc.Authorization
{
    public class ServiceTokenAuthenticationOptions : AuthenticationSchemeOptions
    {
        public ulong RequestMaxAgeSeconds { get; set; }

        public string AuthenticationSchema { get; set; }
    }
}
