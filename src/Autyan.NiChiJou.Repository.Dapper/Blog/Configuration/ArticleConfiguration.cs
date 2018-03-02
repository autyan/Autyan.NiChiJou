using Autyan.NiChiJou.Model.Blog;

namespace Autyan.NiChiJou.Repository.Dapper.Blog.Configuration
{
    public class ArticleConfiguration : DatabaseItemConfiguration<Article>
    {
        public ArticleConfiguration()
        {
            this["Title"].HasMaxLength(100);
            this["Extract"].HasMaxLength(200);
        }
    }
}
