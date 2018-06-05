using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Autyan.NiChiJou.Blog.Controllers
{
    [AllowAnonymous]
    public class ErrorsController : Controller
    {
        private readonly IMemoryCache _cache;

        public ErrorsController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [Route("Errors/Error404")]
        public IActionResult ErrorNotFound()
        {
            return View();
        }

        [Route("Errors/Error500")]
        public IActionResult ErrorInternalException(string requestId)
        {
            if (_cache.TryGetValue(requestId, out var option))
            {
                _cache.Remove(requestId);
            }
            return View(option);
        }
    }
}