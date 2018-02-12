using System;
using System.Collections.Generic;
using System.Security.Claims;
using Autyan.NiChiJou.Core.Config;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.Core.Mvc.Extension
{
    public static class ControllerExtension
    {
        public static async void CookieLoginAsync(this Controller controller, string sessionId)
        {
            var claims = new List<Claim>
            {
                new Claim("SessionId", sessionId)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, ResourceConfiguration.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.UtcNow
            };

            await controller.HttpContext.SignInAsync(
                ResourceConfiguration.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}
