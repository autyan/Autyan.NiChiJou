using System.ComponentModel;

namespace Autyan.NiChiJou.Service.Identity
{
    public enum SessionServiceError
    {
        [Description("User Id Is Null!")]
        UserIdIsNull = 1001,

        [Description("Session Not Found!")]
        SessionNotFound = 2001
    }
}
