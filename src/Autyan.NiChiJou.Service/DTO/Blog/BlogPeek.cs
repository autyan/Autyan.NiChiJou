using System;
using System.Collections.Generic;
using System.Text;

namespace Autyan.NiChiJou.Service.DTO.Blog
{
    public class BlogPeek
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public string Extract { get; set; }

        public DateTimeOffset CreateAt { get; set; }

        public DateTimeOffset? ModifyAt { get; set; }
    }
}
