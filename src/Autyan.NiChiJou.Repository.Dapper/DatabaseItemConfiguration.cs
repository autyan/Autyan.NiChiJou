using Autyan.NiChiJou.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Autyan.NiChiJou.Core.Extension;
using Humanizer;

namespace Autyan.NiChiJou.Repository.Dapper
{
    public abstract class DatabaseItemConfiguration<T> : DatabaseItemConfiguration where T : BaseEntity
    {
        public Type ItemType => typeof(T);

        protected IEnumerable<PropertyDefinition> PropertyDefinitions { get; set; }

        public string Key { get; set; }

        public DatabaseGeneratedOption KeyOption { get; set; }

        protected DatabaseItemConfiguration()
        {
            var dbColumnProperties = ItemType
                .GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public)
                .Where(p => DatabaseTypes.Contains(p.PropertyType)).ToList();
            var keyColumn = dbColumnProperties.FirstOrDefault(p => p.HasAttribute(typeof(KeyAttribute)));
            if (keyColumn != null)
            {
                Key = keyColumn.Name;
                KeyOption =
                    ((DatabaseGeneratedAttribute) keyColumn.GetAttributeValue(typeof(DatabaseGeneratedAttribute)))
                    .DatabaseGeneratedOption;
            }
            PropertyDefinitions = dbColumnProperties
                .Where(p => p.Name != Key).Select(p => new PropertyDefinition
                {
                    Name = p.Name,
                    PropertyInfo = p
                });
        }

        protected PropertyDefinition this[string name]
        {
            get { return PropertyDefinitions.FirstOrDefault(p => p.Name == name); }
        }

        public DatabaseModelMetadata BuildMetadata()
        {
            return new DatabaseModelMetadata
            {
                Key = Key,
                KeyOption = KeyOption,
                TableName = ItemType.Name.Pluralize(),
                Properties = PropertyDefinitions,
                Columns = PropertyDefinitions.Select(p => p.Name).ToList(),
            };
        }
    }

    public class DatabaseItemConfiguration
    {
        protected static readonly Type[] DatabaseTypes = {
            typeof (int), typeof (long), typeof (byte), typeof (bool), typeof (short), typeof (string),typeof(decimal),
            typeof (int?), typeof (long?), typeof (byte?), typeof (bool?), typeof (short?),typeof(decimal?),
            typeof (DateTime), typeof (DateTime?)
        };
    }
}
