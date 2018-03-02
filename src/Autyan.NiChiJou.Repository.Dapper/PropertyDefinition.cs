using System.Reflection;

namespace Autyan.NiChiJou.Repository.Dapper
{
    public class PropertyDefinition
    {
        private const int DefaultStringLength = 4000;

        public string Name { get; set; }

        public int MaxLength { get; private set; } = DefaultStringLength;

        public PropertyInfo PropertyInfo { get; set; }

        public PropertyDefinition HasMaxLength(int maxlength)
        {
            MaxLength = maxlength;
            return this;
        }
    }
}
