using System.Data;

namespace Autyan.NiChiJou.Core.Repository
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection(string name);
    }
}
