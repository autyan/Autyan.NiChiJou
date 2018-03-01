using System.Threading.Tasks;
using Autyan.NiChiJou.Service.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.Blog.Controllers
{
    [AllowAnonymous]
    public class BlogController : Controller
    {
        private IBlogService BlogService { get; }

        public BlogController(IBlogService blogService)
        {
            BlogService = blogService;
        }

        [Route("Blog/{blogName}")]
        public async Task<IActionResult> UserBlog(string blogName)
        {
            var blogIndex = await BlogService.LoadBlogByNameAsync(blogName);
            if (!blogIndex.Succeed)
            {
                return NotFound();
            }

            return View(blogIndex.Data);
        }
    }
}