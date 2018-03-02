using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.Blog.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult ErrorNotFound()
        {
            return View();
        }
    }
}