using Autyan.NiChiJou.IdentityServer.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.IdentityServer.Controllers
{
    [Authorize(Policy = AuthorizePolicy.InternalServiceOnly)]
    public class MemberController : Controller
    {

    }
}