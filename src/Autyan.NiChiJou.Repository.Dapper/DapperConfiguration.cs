using Autyan.NiChiJou.Core.Repository;
using Autyan.NiChiJou.Core.Repository.DbConnectionFactory;
using Autyan.NiChiJou.Core.Utility.Sql;

namespace Autyan.NiChiJou.Repository.Dapper
{
    public class DapperConfiguration
    {
        public static DapperConfiguration Instance { get; private set; } = new DapperConfiguration();

        private DapperConfiguration()
        {
        }

        public IDbConnectionFactory DbConnectionFactory { get; private set; }

        public ISqlBuilderFactory SqlBuilderFactory { get; private set; }

        public static void UseMssql()
        {
            Instance.DbConnectionFactory = new MsSqlDbConnectionFactory();
            Instance.SqlBuilderFactory = new MsSqlBuilderFactory();
        }

        public static void UseMySql()
        {
            Instance.DbConnectionFactory = new MySqlDbConnectionFactory();
            Instance.SqlBuilderFactory = new MySqlBuilderFactory();
        }
    }
}
