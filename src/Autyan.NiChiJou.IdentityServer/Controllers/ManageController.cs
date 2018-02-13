using Microsoft.AspNetCore.Mvc;

namespace Autyan.NiChiJou.IdentityServer.Controllers
{
    public class ManageController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}