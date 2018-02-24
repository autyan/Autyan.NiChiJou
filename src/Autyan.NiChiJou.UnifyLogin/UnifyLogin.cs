using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Autyan.NiChiJou.UnifyLogin
{
    public class UnifyLogin
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnifyLogin(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void HandlerLogin(string memberCode)
        {
            _httpContextAccessor.HttpContext.SignInAsync(null);
        }
    }
}
