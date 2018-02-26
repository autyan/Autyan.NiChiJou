using System.Threading.Tasks;
using Autyan.NiChiJou.UnifyLogin;
using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.Blog.Controllers
{
    public class AuthController : Controller
    {
        private LoginAction LoginAction { get; }

        public AuthController(LoginAction action)
        {
            LoginAction = action;
        }

        public async Task<IActionResult> MemberLogin(string token)
        {
            var loginSucceed = await LoginAction.VerifySecurityToken(token);
            if (loginSucceed)
            {
                await LoginAction.CookieLogin(null);
            }
            return Redirect("/");
        }
    }
}