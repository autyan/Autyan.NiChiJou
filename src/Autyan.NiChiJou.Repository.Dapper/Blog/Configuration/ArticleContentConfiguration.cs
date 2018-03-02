using Autyan.NiChiJou.Model.Blog;

namespace Autyan.NiChiJou.Repository.Dapper.Blog.Configuration
{
    public class ArticleContentConfiguration : DatabaseItemConfiguration<ArticleContent>
    {
        public ArticleContentConfiguration()
        {
            this["Content"].HasMaxLength(-1);
        }
    }
}
