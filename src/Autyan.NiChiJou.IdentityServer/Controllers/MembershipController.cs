﻿using System.Threading.Tasks;
using Autyan.NiChiJou.IdentityServer.Consts;
using Autyan.NiChiJou.Service.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.IdentityServer.Controllers
{
    [Authorize(Policy = AuthorizePolicy.InternalServiceOnly)]
    public class MembershipController : Controller
    {
        private IMembershipService MembershipService { get; }

        public MembershipController(IMembershipService membershipService)
        {
            MembershipService = membershipService;
        }

        public async Task<IActionResult> MemberInfo(string sessionId)
        {
            var membership = await MembershipService.FindMemberBySessionIdAsync(sessionId);
            return Json(membership);
        }
    }
}