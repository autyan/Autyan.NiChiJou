using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Context;
using Autyan.NiChiJou.Core.Options;
using Autyan.NiChiJou.Service.Blog;
using Autyan.NiChiJou.Service.Blog.ServiceStatusCode;
using Autyan.NiChiJou.Service.DTO.Blog;
using Autyan.NiChiJou.UnifyLogin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Autyan.NiChiJou.Blog.Controllers
{
    public class AuthController : Controller
    {
        private Passport Passport { get; }

        private IBlogUserService BlogUserService { get; }

        private IIdentityContext<BlogIdentity> IdentityContext { get; }

        private IOptions<AutyanCookieOptions> Options { get; }

        public AuthController(Passport action,
            IBlogUserService blogUserService,
            IIdentityContext<BlogIdentity> identityContext,
            IOptions<AutyanCookieOptions> options)
        {
            Passport = action;
            BlogUserService = blogUserService;
            IdentityContext = identityContext;
            Options = options;
        }

        [AllowAnonymous]
        public async Task<IActionResult> MemberLogin(string token)
        {
            var loginSucceed = await Passport.VerifySecurityTokenAsync(token);
            if (loginSucceed)
            {
                var findResult = await BlogUserService.FindBlogUserByMemberCodeAsync(Passport.Member.MemberCode);
                if (!findResult.Succeed && findResult.ErrorCode == (int)BlogUserStatus.BlogUserNotExists)
                {
                    findResult = await BlogUserService.CreateBlogUserAsync(Passport.Member.NikeName, Passport.Member.MemberCode);
                }
                var blogUser = findResult.Data;
                var identity = await BlogUserService.CreateBlogIdentityAsync(blogUser);
                await IdentityContext.SetIdentityAsync(Passport.Member.MemberCode, identity.Data);
                await Passport.CookieLogin(null);
            }
            return Redirect("/");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return Redirect(Options.Value.RegisterPath);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return Redirect(Options.Value.LoginPath);
        }

        public async Task<IActionResult> Logout()
        {
            await Passport.CookieLogoutAsync();
            return Redirect(Options.Value.LogoutPath);
        }
    }
}