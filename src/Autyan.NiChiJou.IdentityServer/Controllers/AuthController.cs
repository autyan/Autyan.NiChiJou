using System.Threading.Tasks;
using Autyan.NiChiJou.BusinessModel.Identity;
using Autyan.NiChiJou.Core.Mvc.Extension;
using Autyan.NiChiJou.IdentityServer.Models.Auth;
using Autyan.NiChiJou.Service.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly ISignInService _signInService;

        public AuthController(ISignInService signInService)
        {
            _signInService = signInService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string businessId)
        {
            return View(new LoginViewModel
            {
                BusinessId = businessId
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var signInResult = await _signInService.BusinessSystemPasswordSignIn(model.LoginName, model.Password, model.BusinessId);
            if (signInResult.Succeed) return RedirectToAction(nameof(BusinessLoginEnd), new BusinessSystemSignInModel
            {
                SessionId = signInResult.Data.SessionId,
                BusinessDomainUrl = signInResult.Data.BusinessDomainUrl
            });
            foreach (var message in signInResult.Messages)
            {
                ModelState.AddModelError(string.Empty, message);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult BusinessLoginEnd(BusinessSystemSignInModel model)
        {
            this.CookieLoginAsync(model.SessionId);
            var targetModel = new SignInRedirectViewModel
            {
                TargetUrl = string.IsNullOrEmpty(model.BusinessDomainUrl)
                    ? "/"
                    : $"https://{model.BusinessDomainUrl}?sessionId={model.SessionId}"
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
            var registerResult = await _signInService.RegisterSignInAsync(new UserRegisterModel
            {
                LoginName = model.LoginName,
                Password = model.Password
            });

            if (registerResult.Succeed)
            {
                this.CookieLoginAsync(registerResult.Data);
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