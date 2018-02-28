using System.Threading.Tasks;

namespace Autyan.NiChiJou.Core.Context
{
    public interface IIdentityContext<T> where T : class, new()
    {
        T Ientity { get; }

        Task SetIdentityAsync(string key, T identity);
    }
}
