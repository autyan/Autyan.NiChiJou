using System;
using System.Collections.Generic;

namespace Autyan.NiChiJou.Core.Mvc.Models
{
    public class JsonResponse
    {
        public object Data { get; set; }

        public string RequestId { get; set; }

        public List<string> Messages { get; set; } = new List<string>();

        public Exception Exception { get; set; }
    }
}
