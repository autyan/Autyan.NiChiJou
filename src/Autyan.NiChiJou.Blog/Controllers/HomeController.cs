using Autyan.NiChiJou.Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Autyan.NiChiJou.Blog.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = new PagedArticleQueryViewModel
            {
                Skip = 0,
                Take = 10
            };
            return View(model);
        }
    }
}
