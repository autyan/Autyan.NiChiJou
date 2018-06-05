using System;
using System.ComponentModel.DataAnnotations;

namespace Autyan.NiChiJou.Core.Mvc.Options
{
    public class UnhandledExceptionOptions
    {
        [Display(Name = "RequestId")]
        public string RequestId { get; set; }

        [Display(Name = "ExceptionDetail")]
        public Exception Exception { get; set; }
    }
}
