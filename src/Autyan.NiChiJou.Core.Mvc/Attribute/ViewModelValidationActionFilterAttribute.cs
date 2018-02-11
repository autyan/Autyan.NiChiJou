using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Autyan.NiChiJou.Core.Mvc.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ViewModelValidationActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;
            var controller = (Controller)context.Controller;
            context.Result = new ViewResult
            {
                ViewData = controller.ViewData,
                TempData = controller.TempData
            };
        }
    }
}
