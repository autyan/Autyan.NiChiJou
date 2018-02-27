using Autyan.NiChiJou.Core.Repository;
using Autyan.NiChiJou.Core.Utility.Sql;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Repository.Blog;

namespace Autyan.NiChiJou.Repository.Dapper.Blog
{
    public class ArticleRepository : LongKeyDapperRepository<Article>, IArticleRepository
    {
        protected ArticleRepository(IDbConnectionFactory dbConnectionFactory, ISqlBuilderFactory sqlBuilderFactory) : base(dbConnectionFactory, sqlBuilderFactory)
        {
        }
    }
}
