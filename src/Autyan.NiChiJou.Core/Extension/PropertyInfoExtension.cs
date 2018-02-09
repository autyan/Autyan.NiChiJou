using System;
using System.Linq;
using System.Reflection;

namespace Autyan.NiChiJou.Core.Extension
{
    public static class PropertyInfoExtension
    {
        public static bool HasAttribute(this PropertyInfo propertyInfo, Type attributeType)
        {
            return Attribute.IsDefined(propertyInfo, attributeType);
        }

        public static object GetAttributeValue(this PropertyInfo propertyInfo, Type attributeType, bool inherit = false)
        {
            var attrs = propertyInfo.GetCustomAttributes(attributeType, inherit);
            return attrs?.First();
        }
    }
}
