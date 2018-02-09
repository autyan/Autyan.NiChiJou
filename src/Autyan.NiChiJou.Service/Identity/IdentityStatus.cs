using System.ComponentModel;

namespace Autyan.NiChiJou.Service.Identity
{
    public enum IdentityStatus
    {
        [Description("LoginName Exist!")]
        LoginNameExists = 1001,

        [Description("Email Registered!")]
        EmailRegistered = 1002,

        [Description("PhoneNumber Registered!")]
        PhoneNumberRegistered = 1003
    }
}
