namespace Autyan.NiChiJou.DTO.Blog
{
    public class BlogIndex
    {
        public long? BlogUserId { get;set; }

        public long? BlogId { get; set; }

        public string BlogName { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }
    }
}
