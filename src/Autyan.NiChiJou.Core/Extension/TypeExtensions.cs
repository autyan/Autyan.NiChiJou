using System;

namespace Autyan.NiChiJou.Core.Extension
{
    public static class TypeExtensions
    {
        public static bool IsQueryTypes(this Type type)
        {
            if (type.IsPrimitive || type == typeof(string) || type.IsArray || type == typeof(DateTime)
                || type == typeof(DateTimeOffset) || type == typeof(Enum))
            {
                return true;
            }

            var underlyingType = Nullable.GetUnderlyingType(type);
            return underlyingType?.IsPrimitive == true;
        }

        public static bool HasBaseType(this Type type, Type targetType)
        {
            var currentType = type;
            while (currentType.BaseType != null)
            {
                var baseType = currentType.BaseType;
                if (baseType == targetType) return true;
                currentType = currentType.BaseType;
            }

            return false;
        }
    }
}
