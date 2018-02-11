using System.Threading.Tasks;
using Autyan.NiChiJou.BusinessModel.Identity;
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
            var signInResult = await _signInServcice.BusinessSystemPasswordSignIn(model.LoginName, model.Password, model.BusinessId);
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
        public IActionResult BusinessLoginEnd([FromRoute]BusinessSystemSignInModel model)
        {
            Response.Cookies.Append("Autyan_SessionId", model.SessionId);
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
    }
}