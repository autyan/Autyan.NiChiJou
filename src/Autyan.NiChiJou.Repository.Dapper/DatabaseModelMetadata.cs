using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Autyan.NiChiJou.Repository.Dapper
{
    public class DatabaseModelMetadata
    {
        public KeyInfomation Key { get; set; }

        public string TableName { get; set; }

        public IEnumerable<string> Columns { get; set; }

        public IEnumerable<PropertyInfo> PropertyInfos { get; set; }

        public IEnumerable<PropertyDefinition> Properties { get; set; }
    }

    public class PropertyDefinition
    {
        public string Name { get; set; }

        public PropertyInfo PropertyInfo { get; set; }
    }

    public class KeyInfomation
    {
        public string ColumnName { get; set; }

        public DatabaseGeneratedOption Option { get; set; }
    }
}
