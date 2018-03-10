using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Mvc.Models;

namespace Autyan.NiChiJou.Core.Mvc.Authorization
{
    public interface IServiceTokenProvider
    {
        Task<InternalServiceToken> FindServiceByNameAsync(string name);

        Task<InternalServiceToken> FindServiceByAppIdAsync(string id);
    }
}
