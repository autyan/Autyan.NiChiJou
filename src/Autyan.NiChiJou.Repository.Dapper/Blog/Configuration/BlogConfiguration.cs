namespace Autyan.NiChiJou.Repository.Dapper.Blog.Configuration
{
    public class BlogConfiguration : DatabaseItemConfiguration<Model.Blog.Blog>
    {
        public BlogConfiguration()
        {
            this["BlogName"].HasMaxLength(200);
            this["Description"].HasMaxLength(200);
        }
    }
}
