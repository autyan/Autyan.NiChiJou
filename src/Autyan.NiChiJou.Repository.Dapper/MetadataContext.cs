using System;
using System.Collections.Generic;
using System.Reflection;
using Autyan.NiChiJou.Core.Component;
using Autyan.NiChiJou.Core.Extension;

namespace Autyan.NiChiJou.Repository.Dapper
{
    public class MetadataContext
    {
        private MetadataContext()
        {

        }

        private static readonly IDictionary<Type, DatabaseModelMetadata> MetadataMapping = new Dictionary<Type, DatabaseModelMetadata>();

        public static MetadataContext Instance { get; } = new MetadataContext();

        public DatabaseModelMetadata this[Type type] => MetadataMapping.ContainsKey(type) ? MetadataMapping[type] : null;

        public void Initilize(Assembly[] assemblies)
        {
            var finder = TypeFinder.Scope(assemblies);
            foreach (var type in finder.Find(t => !t.IsAbstract && t.IsClass && t.HasBaseType(typeof(DatabaseItemConfiguration))))
            {
                var instance = Activator.CreateInstance(type);
                var tableType = (Type)type.GetProperty("ItemType").GetValue(instance);
                MetadataMapping[tableType] = (DatabaseModelMetadata)type.GetMethod("BuildMetadata").Invoke(instance, null);
            }
        }
    }
}
