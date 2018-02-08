using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Autyan.NiChiJou.Core.Repository
{
    public class DefaultDbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection GetConnection(string name)
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings[name].ConnectionString);
        }
    }
}
