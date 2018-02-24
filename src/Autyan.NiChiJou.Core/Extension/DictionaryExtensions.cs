using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Autyan.NiChiJou.Core.Extension
{
    public static class DictionaryExtensions
    {
        public static IDictionary<string, string> ToKeyValue(this object metaToken)
        {
            if (metaToken == null)
                return null;
            if (!(metaToken is JToken jtoken))
                return JObject.FromObject(metaToken).ToKeyValue();
            if (jtoken.HasValues)
            {
                var first = new Dictionary<string, string>();
                first = jtoken.Children().ToList()
                    .Select(metaToken1 => metaToken1.ToKeyValue())
                    .Where(keyValue => keyValue != null)
                    .Aggregate(first, (current, keyValue) => current.Concat(keyValue).ToDictionary(k => k.Key, v => v.Value));
                return first;
            }
            var jvalue = jtoken as JValue;
            if (jvalue != null && jvalue.Value == null)
            {
                return null;
            }
            if (jvalue == null) return null;
            var str = jvalue.Type == JTokenType.Date
                ? jvalue.ToString("o", CultureInfo.InvariantCulture)
                : jvalue.ToString(CultureInfo.InvariantCulture);
            return new Dictionary<string, string>
            {
                {
                    jtoken.Path,
                    str
                }
            };
        }
    }
}
