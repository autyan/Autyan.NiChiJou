using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Autyan.NiChiJou.Core.Mvc
{
    public class ViewModelValidationActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;
            var controller = (Controller) context.Controller;
            context.Result = new ViewResult
            {
                ViewData = controller.ViewData,
                TempData = controller.TempData
            };
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}
