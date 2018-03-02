using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Autyan.NiChiJou.Repository.Dapper
{
    public class DatabaseModelMetadata
    {
        public string Key { get; set; }

        public string TableName { get; set; }

        public IEnumerable<string> Columns { get; set; }

        public IEnumerable<PropertyDefinition> Properties { get; set; }

        public DatabaseGeneratedOption KeyOption { get; set; }
    }
}
