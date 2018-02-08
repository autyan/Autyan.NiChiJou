using System.ComponentModel;

namespace Autyan.NiChiJou.Model.Enumeration
{
    public enum ClientType : byte
    {
        [Description("网站")]
        Web,

        [Description("安卓")]
        Android,

        [Description("iOS")]
        Ios,

        [Description("UwpApp")]
        Uwp
    }
}
