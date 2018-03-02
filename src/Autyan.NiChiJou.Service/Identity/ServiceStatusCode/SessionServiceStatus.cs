using System.ComponentModel;

namespace Autyan.NiChiJou.Service.Identity.ServiceStatusCode
{
    public enum SessionServiceStatus
    {
        [Description("User Id Is Null!")]
        UserIdIsNull = 1001,

        [Description("Session Not Found!")]
        SessionNotFound = 2000
    }
}
