using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Autyan.NiChiJou.Core.Component;
using Autyan.NiChiJou.Core.Data;
using Autyan.NiChiJou.Core.Extension;
using Humanizer;

namespace Autyan.NiChiJou.Repository.Dapper
{
    public class MetadataContext
    {
        private MetadataContext()
        {

        }

        private static readonly IDictionary<Type, DatabaseModelMetadata> MetadataMapping = new Dictionary<Type, DatabaseModelMetadata>();

        private static readonly Type[] DatabaseTypes = {
            typeof (int), typeof (long), typeof (byte), typeof (bool), typeof (short), typeof (string),typeof(decimal),
            typeof (int?), typeof (long?), typeof (byte?), typeof (bool?), typeof (short?),typeof(decimal?),
            typeof (DateTime), typeof (DateTime?), typeof(DateTimeOffset), typeof(DateTimeOffset?)
        };

        public static MetadataContext Instance { get; } = new MetadataContext();

        public DatabaseModelMetadata this[Type type] => MetadataMapping.ContainsKey(type) ? MetadataMapping[type] : null;

        public void Initilize(Assembly[] assemblies)
        {
            var finder = TypeFinder.Scope(assemblies);
            foreach (var type in finder.Find(t => !t.IsAbstract && t.IsClass && t.HasBaseType(typeof(BaseEntity))))
            {
                var properties =
                    type.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
                var columns = new List<string>();
                var keyColumn = properties.FirstOrDefault(p => p.HasAttribute(typeof(KeyAttribute)));
                if (keyColumn == null) throw new ArgumentException("DataObject Has No Key Column");
                var keyOption = new KeyInfomation
                {
                    ColumnName = keyColumn.Name,
                    Option =
                        ((DatabaseGeneratedAttribute) keyColumn.GetAttributeValue(typeof(DatabaseGeneratedAttribute)))
                        .DatabaseGeneratedOption
                };
                columns.AddRange(properties.Where(p => DatabaseTypes.Contains(p.PropertyType)).Select(p => p.Name));
                MetadataMapping[type] = new DatabaseModelMetadata
                {
                    TableName = type.Name.Pluralize(),
                    Columns = columns,
                    Key = keyOption
                };
            }
        }
    }
}
