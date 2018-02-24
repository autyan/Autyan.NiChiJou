using System.Data;
using System.Data.SqlClient;

namespace Autyan.NiChiJou.Core.Repository.DbConnectionFactory
{
    public class MsSqlDbConnectionFactory : BaseDbConnectionFactory, IDbConnectionFactory
    {
        public IDbConnection GetConnection(string name)
        {
            return new SqlConnection(GetConnectionString(name));
        }
    }
}
