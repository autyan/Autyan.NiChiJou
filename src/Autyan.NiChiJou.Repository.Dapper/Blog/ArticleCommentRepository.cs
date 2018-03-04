using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Repository;
using Autyan.NiChiJou.Core.Utility.Sql;
using Autyan.NiChiJou.DTO.Blog;
using Autyan.NiChiJou.Model.Blog;
using Autyan.NiChiJou.Repository.Blog;
using Dapper;

namespace Autyan.NiChiJou.Repository.Dapper.Blog
{
    public class ArticleCommentRepository : LongKeyDapperRepository<ArticleComment>, IArticleCommentRepository
    {
        public ArticleCommentRepository(IDbConnectionFactory dbConnectionFactory, ISqlBuilderFactory sqlBuilderFactory) : base(dbConnectionFactory, sqlBuilderFactory)
        {
        }

        public async Task<IEnumerable<ArticleCommentDetails>> LoadArticleCommentDetailsAsync(ArticleCommentQuery query)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT Content, ToComment, BlogUsers.NickName AS CommentedBy, PostId, ArticleComments.CreatedAt ")
                .Append(" FROM ArticleComments LEFT JOIN BlogUsers ON CommentedBy = BlogUsers.Id ")
                .Append(" WHERE 1=1 ");
            if (query.Id != null)
            {
                queryBuilder.Append(" AND ArticleComments.Id = @Id");
            }

            if (query.PostId != null)
            {
                queryBuilder.Append(" AND PostId = @PostId");
            }

            return await Connection.QueryAsync<ArticleCommentDetails>(queryBuilder.ToString(), query);
        }
    }
}
