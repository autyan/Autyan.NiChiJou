using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Mvc.Authorization;
using Autyan.NiChiJou.IdentityServer.Consts;
using Autyan.NiChiJou.IdentityServer.Models.Auth;
using Autyan.NiChiJou.Service.DTO.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager SignInManager { get; }

        public AuthController(SignInManager signInService)
        {
            SignInManager = signInService;
            SignInManager.Controller = this;
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
            var signInResult = await SignInManager.PasswordSignInAsync(model.LoginName, model.Password);
            if (!signInResult.Succeed)
            {
                foreach (var message in signInResult.Messages)
                {
                    ModelState.AddModelError(string.Empty, message);
                }
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.ReturnUrl))
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(
                $"http://{model.ReturnUrl}?token={SignInManager.CreateLoginVerificationToken(signInResult.Data.SessionId)}");
        }

        [AllowAnonymous]
        public IActionResult UnifySignIn(string returnUrl)
        {
            if (SignInManager.IsSignedIn())
            {
                return Redirect(
                    $"http://{returnUrl}?token={SignInManager.CreateLoginVerificationToken()}");
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            var registerResult = await SignInManager.RegisterUserAsync(new UserRegistration
            {
                LoginName = model.LoginName,
                Password = model.Password
            });

            if (registerResult.Succeed)
            {
                await SignInManager.PasswordSignInAsync(model.LoginName, model.Password);
                return RedirectToAction("Index", "Home");
            }
            foreach (var message in registerResult.Messages)
            {
                ModelState.AddModelError(string.Empty, message);
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = AuthorizePolicy.InternalServiceOnly)]
        public async Task<IActionResult> VerifiToken(TokenVerificationViewMoodel model)
        {
            var signInResult = await SignInManager.GetSessionIdByVerificationTokenAsync(model.Token);
            return Json(signInResult.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}