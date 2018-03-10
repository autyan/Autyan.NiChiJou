using System.Threading.Tasks;
using Autyan.NiChiJou.DTO.Identity;
using Autyan.NiChiJou.IdentityServer.Consts;
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
            _signInManager.Controller = this;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, [FromQuery]UnifySignInViewModel unifySignIn)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(model.LoginName, model.Password);
            if (!signInResult.Succeed)
            {
                foreach (var message in signInResult.Messages)
                {
                    ModelState.AddModelError(string.Empty, message);
                }
                return View(model);
            }

            return await _signInManager.SignInRedirectAsync(unifySignIn.ReturnUrl, unifySignIn.SubSystem, signInResult.Data.SessionId);
        }

        [AllowAnonymous]
        public async Task<IActionResult> UnifySignIn(UnifySignInViewModel model)
        {
            if (_signInManager.IsSignedIn())
            {
                var token = await _signInManager.CreateLoginVerificationTokenAsync();
                return Redirect(
                    $"{model.ReturnUrl}?token={token.Data}");
            }

            return RedirectToAction(nameof(Login), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            var registerResult = await _signInManager.RegisterUserAsync(new UserRegistration
            {
                LoginName = model.LoginName,
                Password = model.Password,
                InviteCode = model.InviteCode
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

        [Authorize(Policy = AuthorizePolicy.InternalServiceOnly)]
        public async Task<IActionResult> VerifiToken(TokenVerificationViewMoodel model)
        {
            var signInResult = await _signInManager.GetSessionIdByVerificationTokenAsync(model.Token);
            return Content(signInResult.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            await HttpContext.SignOutAsync();
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> MemberLogout(string returnUrl)
        {
            await HttpContext.SignOutAsync();
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(Login));
        }
    }
}