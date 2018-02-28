﻿using Autyan.NiChiJou.Model.Blog;

namespace Autyan.NiChiJou.Repository.Dapper.Blog.Configuration
{
    public class ArticleCommentConfiguration : DatabaseItemConfiguration<ArticleComment>
    {
        public ArticleCommentConfiguration()
        {
            this["Content"].HasMaxLength(2000);
        }
    }
}
