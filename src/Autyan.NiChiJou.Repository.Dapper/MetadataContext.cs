using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Autyan.NiChiJou.Core.Data;
using Humanizer;

namespace Autyan.NiChiJou.Repository.Dapper
{
    public class MetadataContext
    {
        private static readonly ConcurrentDictionary<Type, MetaData> EntityMetaDataCache = new ConcurrentDictionary<Type, MetaData>();

        private static readonly ConcurrentDictionary<Type, MetaData> DynamicParamMetaDataCache = new ConcurrentDictionary<Type, MetaData>();

        private MetadataContext()
        {

        }

        public static MetadataContext Instance { get; } = new MetadataContext();

        public MetaData this[Type type]
        {
            get
            {
                if (EntityMetaDataCache.ContainsKey(type))
                {
                    return EntityMetaDataCache[type];
                }

                if (DynamicParamMetaDataCache.ContainsKey(type))
                {
                    return DynamicParamMetaDataCache[type];
                }

                var metaData = MetadataGenerator(type);
                if (typeof(BaseEntity).IsAssignableFrom(type))
                {
                    EntityMetaDataCache.TryAdd(type, metaData);
                }
                else
                {
                    DynamicParamMetaDataCache.TryAdd(type, metaData);
                }
                return metaData;
            }
        }

        private static MetaData MetadataGenerator(Type type)
        {
            var typeProperties = type.GetProperties().Where(p => DatabaseTypes.Contains(p.PropertyType)).ToList();
            var metadata = new MetaData
            {
                EntityType = type,
                TableName = type.Name.Pluralize(),
                Columns = typeProperties.Select(p => p.Name),
                PropertyInfos = typeProperties
            };
            return metadata;
        }

        public static void PreInitialEntities(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var metadata = MetadataGenerator(type);
                EntityMetaDataCache.TryAdd(type, metadata);
            }
        }

        private static readonly Type[] DatabaseTypes =
        {
            typeof(int), typeof(int?), typeof(string), typeof(Enum), typeof(bool), typeof(bool?), typeof(long), typeof(long?),
            typeof(DateTimeOffset), typeof(DateTimeOffset?), typeof(double), typeof(double?), typeof(float), typeof(float?),
            typeof(DateTime), typeof(DateTime?), typeof(decimal), typeof(decimal?), typeof(short), typeof(short?), typeof(byte[])
        };
    }
}
