using System.Threading.Tasks;
using Autyan.NiChiJou.Blog.Models;
using Autyan.NiChiJou.Service.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.Blog.Controllers
{
    [AllowAnonymous]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [Route("Blog/{blogName}")]
        public async Task<IActionResult> UserBlog(string blogName, PagedQueryViewModel model)
        {
            var blogIndex = await _blogService.LoadBlogByNameAsync(blogName);
            if (!blogIndex.Succeed)
            {
                return NotFound();
            }

            if (model.Take == null)
            {
                model.Take = 5;
            }
            blogIndex.Data.Skip = model.Skip;
            blogIndex.Data.Take = model.Take;
            return View(blogIndex.Data);
        }
    }
}