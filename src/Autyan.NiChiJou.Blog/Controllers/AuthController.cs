using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.Blog.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult MemberLogin(string memberCode)
        {
            return Redirect("/");
        }
    }
}