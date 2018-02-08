using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autyan.NiChiJou.Core.Component;
using Autyan.NiChiJou.Core.Data;
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
            typeof (DateTime),
            typeof (DateTime?)
        };

        public static MetadataContext Instance { get; } = new MetadataContext();

        public DatabaseModelMetadata this[Type type] => MetadataMapping.ContainsKey(type) ? MetadataMapping[type] : null;

        public void Initilize(Assembly[] assemblies)
        {
            var finder = TypeFinder.Scope(assemblies);
            foreach (var type in finder.Find(t => !t.IsAbstract && t.IsClass && t.BaseType == typeof(BaseEntity)))
            {
                var properties =
                    type.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
                var columns = new List<string>();
                if (properties.Any(p => p.Name == "Id"))
                {
                    columns.Add("Id");
                }
                columns.AddRange(properties.Where(p => p.Name != "Id" && DatabaseTypes.Contains(p.PropertyType)).Select(p => p.Name));
                MetadataMapping[type] = new DatabaseModelMetadata
                {
                    TableName = type.Name.Pluralize(),
                    Columns = columns
                };
            }
        }
    }
}
