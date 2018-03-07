using System.Data;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Autyan.NiChiJou.Core.Repository.DbConnectionFactory
{
    public class MySqlDbConnectionFactory : BaseDbConnectionFactory, IDbConnectionFactory
    {
        public MySqlDbConnectionFactory(IConfiguration configuration) : base(configuration)
        {
        }

        public IDbConnection GetConnection(string name)
        {
            return new MySqlConnection(GetConnectionString(name));
        }
    }
}
