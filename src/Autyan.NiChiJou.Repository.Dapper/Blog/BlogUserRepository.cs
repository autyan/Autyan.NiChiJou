using Autyan.NiChiJou.Core.Repository;
using Autyan.NiChiJou.Core.Repository.Blog;
using Autyan.NiChiJou.Core.Utility.Sql;
using Autyan.NiChiJou.Model.Blog;

namespace Autyan.NiChiJou.Repository.Dapper.Blog
{
    public class BlogUserRepository : LongKeyDapperRepository<BlogUser>, IBlogUserRepository
    {
        public BlogUserRepository(IDbConnectionFactory dbConnectionFactory, ISqlBuilderFactory sqlBuilderFactory) : base(dbConnectionFactory, sqlBuilderFactory)
        {
        }
    }
}
