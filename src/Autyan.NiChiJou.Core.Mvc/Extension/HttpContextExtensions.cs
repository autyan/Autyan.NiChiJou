using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Autyan.NiChiJou.Core.Mvc.Extension
{
    public static class HttpContextExtensions
    {
        public static bool IsSignedIn(this HttpContext context)
        {
            if (context.User == null) return false;
            return context.User.Identities.Any(i => i.IsAuthenticated);
        }

        public static bool IsSignedIn(this HttpContext context, string schema)
        {
            if (context.User == null) return false;
            var schemaIdentity = context.User.Identities.FirstOrDefault(i => i.AuthenticationType == schema);
            return schemaIdentity != null && schemaIdentity.IsAuthenticated;
        }
    }
}
