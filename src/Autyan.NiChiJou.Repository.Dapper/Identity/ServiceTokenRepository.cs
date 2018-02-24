using Autyan.NiChiJou.Core.Repository;
using Autyan.NiChiJou.Core.Utility.Sql;
using Autyan.NiChiJou.Model.Identity;
using Autyan.NiChiJou.Repository.Identity;

namespace Autyan.NiChiJou.Repository.Dapper.Identity
{
    public class ServiceTokenRepository : LongKeyDapperRepository<ServiceToken>, IServiceTokenRepository
    {
        public ServiceTokenRepository(IDbConnectionFactory dbConnectionFactory, ISqlBuilderFactory sqlBuilderFactory) : base(dbConnectionFactory, sqlBuilderFactory)
        {
        }
    }
}
