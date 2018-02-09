using System.Data;
using System.Data.SqlClient;

namespace Autyan.NiChiJou.Core.Repository.DbConnectionFactory
{
    public class MySqlDbConnectionFactory : BaseDbConnectionFactory, IDbConnectionFactory
    {
        public IDbConnection GetConnection(string name)
        {
            return new SqlConnection(GetConnectionString(name));
        }
    }
}
