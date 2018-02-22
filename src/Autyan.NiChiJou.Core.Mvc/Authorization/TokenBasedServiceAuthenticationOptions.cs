using Microsoft.AspNetCore.Authentication;

namespace Autyan.NiChiJou.Core.Mvc.Authorization
{
    public class TokenBasedServiceAuthenticationOptions : AuthenticationSchemeOptions
    {
        public ulong RequestMaxAgeSeconds { get; set; }

        public string AuthenticationSchema { get; set; }
    }
}
