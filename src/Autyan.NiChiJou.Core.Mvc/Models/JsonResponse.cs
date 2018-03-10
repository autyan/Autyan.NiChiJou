using System;

namespace Autyan.NiChiJou.Core.Mvc.Models
{
    public class JsonResponse
    {
        public object Data { get; set; }

        public JsonExtraInfo ExtraInfo { get; set; }
    }

    public class JsonExtraInfo
    {
        public string RequestId { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }
    }
}
