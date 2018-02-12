using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Autyan.NiChiJou.Core.Extension
{
    public static class ConfigurationRootSection
    {
        public static string GetValueBySectionAndKey(this IConfigurationRoot configuration, string sectionName, string key)
        {
            return configuration.GetSection(sectionName).GetChildren().FirstOrDefault(c => c.Key == key).Value;
        }

        public static string GetValueFromSectionChildren(this IEnumerable<IConfigurationSection> sections, string key)
        {
            return sections.FirstOrDefault(s => s.Key == key).Value;
        }
    }
}
