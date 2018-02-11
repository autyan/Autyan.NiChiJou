using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Autyan.NiChiJou.Core.Config
{
    public static class ResourceConfiguration
    {
        private static IConfiguration _configuration;

        public static void SetConfigurationRoot(IConfiguration configuration)
        {
            _configuration = configuration;
            Initial();
        }

        private static void Initial()
        {
            _redisConfigs = _configuration.GetSection("RedisServer").GetChildren();
            _sessionConfigs = _configuration.GetSection("Session").GetChildren();
        }

        private static IEnumerable<IConfigurationSection> _redisConfigs;

        private static IEnumerable<IConfigurationSection> _sessionConfigs;

        public static string RedisAddress => _redisConfigs.First(c => c.Key == "Address").Value;

        public static string RedisInstanceName => _redisConfigs.First(c => c.Key == "InstanceName").Value;

        public static double SessionExpiration => double.Parse(_sessionConfigs.First(c => c.Key == "Expiration").Value);

        public static string ConnectionStrings(string name) => _configuration.GetConnectionString(name);
    }
}
