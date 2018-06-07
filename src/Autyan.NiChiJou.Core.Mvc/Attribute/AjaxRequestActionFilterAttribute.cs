using System;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Mvc.Models;
using Autyan.NiChiJou.Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Autyan.NiChiJou.Core.Mvc.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class AjaxRequestActionFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is JsonResult result && !(result.Value is JsonResponse))
            {
                var response = new JsonResponse();
                if (result.Value is ServiceResult serviceResult)
                {
                    response.Messages.AddRange(serviceResult.Messages);
                }
                else
                {
                    response.Data = result.Value;
                }

                result.Value = response;
            }

            await base.OnResultExecutionAsync(context, next);

        }
    }
}
