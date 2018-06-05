using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Context;
using Autyan.NiChiJou.Core.Options;
using Autyan.NiChiJou.DTO.Blog;
using Autyan.NiChiJou.Service.Blog;
using Autyan.NiChiJou.Service.Blog.ServiceStatusCode;
using Autyan.NiChiJou.UnifyLogin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Autyan.NiChiJou.Blog.Controllers
{
    public class AuthController : Controller
    {
        private readonly Passport _passport;

        private readonly IBlogUserService _blogUserService;

        private readonly IIdentityContext<BlogIdentity> _identityContext;

        private readonly IOptions<AutyanCookieOptions> _options;

        public AuthController(Passport action,
            IBlogUserService blogUserService,
            IIdentityContext<BlogIdentity> identityContext,
            IOptions<AutyanCookieOptions> options)
        {
            _passport = action;
            _blogUserService = blogUserService;
            _identityContext = identityContext;
            _options = options;
        }

        [AllowAnonymous]
        public async Task<IActionResult> MemberLogin(string token)
        {
            var loginSucceed = await _passport.VerifySecurityTokenAsync(token);
            if (loginSucceed)
            {
                var findResult = await _blogUserService.FindBlogUserByMemberCodeAsync(_passport.Member.MemberCode);
                if (!findResult.Succeed && findResult.ErrorCode == (int)BlogUserStatus.BlogUserNotExists)
                {
                    findResult = await _blogUserService.CreateBlogUserAsync(_passport.Member.NikeName, _passport.Member.MemberCode);
                }
                var blogUser = findResult.Data;
                var identity = await _blogUserService.CreateBlogIdentityAsync(blogUser);
                await _identityContext.SetIdentityAsync(_passport.Member.MemberCode, identity.Data);
                await _passport.CookieLogin(null);
            }
            return Redirect("/");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return Redirect(_options.Value.RegisterPath);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return Redirect(_options.Value.LoginPath);
        }

        public async Task<IActionResult> Logout()
        {
            await _passport.CookieLogoutAsync();
            return Redirect(_options.Value.LogoutPath);
        }
    }
}