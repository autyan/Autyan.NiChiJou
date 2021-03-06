﻿using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Service;
using Autyan.NiChiJou.DTO.Blog;

namespace Autyan.NiChiJou.Service.Blog
{
    public interface IBlogService
    {
        Task<ServiceResult<BlogIndex>> LoadBlogByNameAsync(string blogName);
    }
}
