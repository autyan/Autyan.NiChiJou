using System.Threading.Tasks;
using Autyan.NiChiJou.IdentityServer.Models.Auth;
using Autyan.NiChiJou.Service.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly ISignInServcice _signInServcice;

        public AuthController(ISignInServcice signInServcice)
        {
            _signInServcice = signInServcice;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _signInServcice.PasswordSignInAsync(model.LoginName, model.Password);
            if (result.Succeed)
            {
                return View();
            }

            foreach (var message in result.Messages)
            {
                ModelState.AddModelError(string.Empty, message);
            }
            return View(model);
        }
    }
}