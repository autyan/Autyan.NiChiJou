using Autyan.NiChiJou.Core.Repository;
using Autyan.NiChiJou.Core.Utility.Sql;
using Autyan.NiChiJou.Repository.Blog;

namespace Autyan.NiChiJou.Repository.Dapper.Blog
{
    public class BlogRepository : LongKeyDapperRepository<Model.Blog.Blog>, IBlogRepository
    {
        protected BlogRepository(IDbConnectionFactory dbConnectionFactory, ISqlBuilderFactory sqlBuilderFactory) : base(dbConnectionFactory, sqlBuilderFactory)
        {
        }
    }
}
