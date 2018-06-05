using System.Threading.Tasks;
using Autyan.NiChiJou.IdentityServer.Consts;
using Autyan.NiChiJou.IdentityServer.Models.Membershhip;
using Autyan.NiChiJou.Service.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.IdentityServer.Controllers
{
    [Authorize(Policy = AuthorizePolicy.InternalServiceOnly)]
    public class MembershipController : Controller
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public async Task<IActionResult> MemberInfo(SessionMemberViewModel model)
        {
            var membership = await _membershipService.FindMemberBySessionIdAsync(model.SessionId);
            return Json(membership.Data);
        }
    }
}