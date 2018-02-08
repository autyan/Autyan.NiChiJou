using System;
using System.Collections.Generic;
using System.Reflection;

namespace Autyan.NiChiJou.Repository.Dapper
{
    public class MetaData
    {
        public Type EntityType { get; set; }

        public string TableName { get; set; }

        public IEnumerable<string> Columns { get; set; }

        public IEnumerable<PropertyInfo> PropertyInfos { get; set; }
    }
}
