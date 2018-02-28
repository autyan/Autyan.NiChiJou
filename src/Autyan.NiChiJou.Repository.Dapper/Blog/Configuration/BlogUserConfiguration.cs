using Autyan.NiChiJou.Model.Blog;

namespace Autyan.NiChiJou.Repository.Dapper.Blog.Configuration
{
    public class BlogUserConfiguration : DatabaseItemConfiguration<BlogUser>
    {
        public BlogUserConfiguration()
        {
            this["NickName"].HasMaxLength(50);
            this["AvatorUrl"].HasMaxLength(200);
            this["MemberCode"].HasMaxLength(50);
        }
    }
}
