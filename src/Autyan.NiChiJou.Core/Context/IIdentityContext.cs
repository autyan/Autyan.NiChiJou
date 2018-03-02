using System.Security.Claims;
using System.Threading.Tasks;

namespace Autyan.NiChiJou.Core.Context
{
    public interface IIdentityContext<T> where T : class, new()
    {
        T Identity { get; }

        ClaimsPrincipal User { get; }

        Task SetIdentityAsync(string key, T identity);
    }
}
