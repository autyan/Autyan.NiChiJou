using Autyan.NiChiJou.Model.Blog;

namespace Autyan.NiChiJou.Repository.Dapper.Blog.Configuration
{
    public class ArticleCommentConfiguration : DatabaseItemConfiguration<ArticleComment>
    {
        public ArticleCommentConfiguration()
        {
            this["Content"].HasMaxLength(4000);
            this["CommentedBy"].HasMaxLength(200);
        }
    }
}
