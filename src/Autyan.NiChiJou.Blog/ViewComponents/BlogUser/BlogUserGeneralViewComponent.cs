using System;
using System.Threading.Tasks;
using Autyan.NiChiJou.Service.Blog;
using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.Blog.ViewComponents.BlogUser
{
    public class BlogUserGeneralViewComponent : ViewComponent
    {
        private IBlogUserService BlogUserService { get; }

        public BlogUserGeneralViewComponent(IBlogUserService blogUserService)
        {
            BlogUserService = blogUserService;
        }

        public async Task<IViewComponentResult> InvokeAsync(long userId)
        {
            var userFindResult = await BlogUserService.FindBlogUserByIdAsync(userId);
            if (!userFindResult.Succeed)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return View(userFindResult.Data);
        }
    }
}
