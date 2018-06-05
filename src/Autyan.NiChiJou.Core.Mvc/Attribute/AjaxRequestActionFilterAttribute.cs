using System;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Autyan.NiChiJou.Core.Mvc.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class AjaxRequestActionFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is JsonResult result)
            {
                result.Value = new JsonResponse
                {
                    Data = result.Value
                };

                return;
            }

            await base.OnResultExecutionAsync(context, next);
        }
    }
}
