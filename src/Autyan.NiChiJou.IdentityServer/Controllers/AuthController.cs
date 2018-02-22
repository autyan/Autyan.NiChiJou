using System.Threading.Tasks;
using Autyan.NiChiJou.BusinessModel.Identity;
using Autyan.NiChiJou.Core.Mvc.Authorization;
using Autyan.NiChiJou.IdentityServer.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager _signInManager;

        public AuthController(SignInManager signInService)
        {
            _signInManager = signInService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(model.LoginName, model.Password);
            if (signInResult.Succeed) return RedirectToAction(nameof(LoginProcess), new LoginProcessModel
            {
                ReturnUrl = model.ReturnUrl,
                SessionId = signInResult.Data.SessionId
            });
            foreach (var message in signInResult.Messages)
            {
                ModelState.AddModelError(string.Empty, message);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult LoginProcess(LoginProcessModel model)
        {
            var targetModel = new SignInRedirectViewModel
            {
                TargetUrl = string.IsNullOrEmpty(model.ReturnUrl)
                    ? "/"
                    : $"http://{model.ReturnUrl}?token={_signInManager.CreateLoginVerificationToken(model.SessionId)}"
            };

            return View(targetModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            var registerResult = await _signInManager.RegisterUserAsync(new UserRegisterModel
            {
                LoginName = model.LoginName,
                Password = model.Password
            });

            if (registerResult.Succeed)
            {
                await _signInManager.PasswordSignInAsync(model.LoginName, model.Password);
                return RedirectToAction("Index", "Home");
            }
            foreach (var message in registerResult.Messages)
            {
                ModelState.AddModelError(string.Empty, message);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}