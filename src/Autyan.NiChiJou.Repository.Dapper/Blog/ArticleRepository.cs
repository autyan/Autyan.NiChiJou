using System.Text;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Data;
using Autyan.NiChiJou.Core.Repository;
using Autyan.NiChiJou.Core.Utility.Sql;
using Autyan.NiChiJou.DTO.Blog;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Repository.Blog;
using Dapper;

namespace Autyan.NiChiJou.Repository.Dapper.Blog
{
    public class ArticleRepository : LongKeyDapperRepository<Article>, IArticleRepository
    {
        public ArticleRepository(IDbConnectionFactory dbConnectionFactory, ISqlBuilderFactory sqlBuilderFactory) : base(dbConnectionFactory, sqlBuilderFactory)
        {
        }

        public override Task<long> InsertAsync(Article entity)
        {
            entity.Reads = 0;
            entity.Comments = 0;
            return base.InsertAsync(entity);
        }

        public async Task<PagedResult<ArticlePreview>> QueryPagedArticlePreviewAsync(ArticleQuery query)
        {
            var count = await GetCountAsync(query);
            var strBuilder = new StringBuilder();
            strBuilder.Append(
                    "SELECT Articles.Id AS Id, Title, Extract, BlogId, Reads, Comments, LastReadAt, BlogUsers.NickName AS Author, c.LastCommentedAt AS LastCommentedAt\r\n")
                .Append("FROM Articles\r\n")
                .Append("LEFT JOIN Blogs ON Articles.BlogId = Blogs.Id\r\n")
                .Append("LEFT JOIN BlogUsers ON Blogs.BlogUserid= BlogUsers.Id\r\n")
                .Append(
                    "LEFT JOIN (SELECT PostId, MAX(CreatedAt) AS LastCommentedAt FROM ArticleComments GROUP BY ArticleComments.PostId) c ON c.PostId = Articles.Id ORDER BY Id ")
                .Append("OFFSET ").Append(query.Skip ?? 0).Append(" ROWS FETCH NEXT ").Append(query.Take)
                .Append(" ROWS ONLY ;");

            var results = await Connection.QueryAsync<ArticlePreview>(strBuilder.ToString(), query);

            return new PagedResult<ArticlePreview>
            {
                TotalCount = count,
                Results = results,
            };
        }
    }
}
