using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Autyan.NiChiJou.Core.Repository.DbConnectionFactory
{
    public class MySqlDbConnectionFactory : BaseDbConnectionFactory, IDbConnectionFactory
    {
        public MySqlDbConnectionFactory(IConfiguration configuration) : base(configuration)
        {
        }

        public IDbConnection GetConnection(string name)
        {
            return new SqlConnection(GetConnectionString(name));
        }
    }
}
