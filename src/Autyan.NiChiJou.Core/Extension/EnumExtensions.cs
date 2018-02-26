using System;
using System.ComponentModel;
using System.Reflection;

namespace Autyan.NiChiJou.Core.Extension
{
    public static class EnumExtensions
    {
        public static bool TryGetDescription(this Enum enumValue, out string description)
        {
            var strValue = enumValue.ToString();
            var enumFailed = enumValue.GetType().GetField(strValue);
            if (!(enumFailed.GetCustomAttribute(typeof(DescriptionAttribute)) is DescriptionAttribute descripAttr))
            {
                description = string.Empty;
                return false;
            }

            description = descripAttr.Description;
            return true;
        }
    }
}
