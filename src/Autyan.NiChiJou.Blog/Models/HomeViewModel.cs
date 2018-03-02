using System.Collections.Generic;
using Autyan.NiChiJou.DTO.Blog;

namespace Autyan.NiChiJou.Blog.Models
{
    public class HomeViewModel
    {
        public IEnumerable<BlogPeek> RecentBlogs { get; set; }
    }
}
