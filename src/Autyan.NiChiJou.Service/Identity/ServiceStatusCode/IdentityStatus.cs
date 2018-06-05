using System.ComponentModel;

namespace Autyan.NiChiJou.Service.Identity.ServiceStatusCode
{
    public enum IdentityStatus
    {
        [Description("LoginName Exist!")]
        LoginNameExists = 1001,

        [Description("Email Registered!")]
        EmailRegistered = 1002,

        [Description("PhoneNumber Registered!")]
        PhoneNumberRegistered = 1003,

        [Description("User Not Found!")]
        UserNotFound = 2000,

        [Description("Invalid Password!")]
        InvalidPassword = 2001,

        [Description("Invalid InviteCode")]
        InvalidInviteCode = 2002,
    }
}
