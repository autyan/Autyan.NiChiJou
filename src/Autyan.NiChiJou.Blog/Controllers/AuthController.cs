using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Context;
using Autyan.NiChiJou.Service.Blog;
using Autyan.NiChiJou.Service.Blog.ServiceStatusCode;
using Autyan.NiChiJou.Service.DTO.Blog;
using Autyan.NiChiJou.UnifyLogin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.Blog.Controllers
{
    public class AuthController : Controller
    {
        private Passport Passport { get; }

        private IBlogUserService BlogUserService { get; }

        private IIdentityContext<BlogIdentity> IdentityContext { get; }

        public AuthController(Passport action,
            IBlogUserService blogUserService,
            IIdentityContext<BlogIdentity> identityContext)
        {
            Passport = action;
            BlogUserService = blogUserService;
            IdentityContext = identityContext;
        }

        [AllowAnonymous]
        public async Task<IActionResult> MemberLogin(string token)
        {
            var loginSucceed = await Passport.VerifySecurityToken(token);
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
    }
}