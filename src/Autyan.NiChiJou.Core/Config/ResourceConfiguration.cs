using System.Collections.Generic;
using Autyan.NiChiJou.Core.Extension;
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
            _authConfigs = _configuration.GetSection("Auth").GetChildren();
        }

        private static IEnumerable<IConfigurationSection> _redisConfigs;

        private static IEnumerable<IConfigurationSection> _sessionConfigs;

        private static IEnumerable<IConfigurationSection> _authConfigs;

        public static string RedisAddress => _redisConfigs.GetValueFromSectionChildren("Address");

        public static string RedisInstanceName => _redisConfigs.GetValueFromSectionChildren("InstanceName");

        public static double RedisDefaultExpiration => double.Parse(_redisConfigs.GetValueFromSectionChildren("DefaultExpiration"));

        public static double SessionExpiration => double.Parse(_sessionConfigs.GetValueFromSectionChildren("Expiration"));

        public static string ConnectionStrings(string name) => _configuration.GetConnectionString(name);

        public static string LoginPath => _authConfigs.GetValueFromSectionChildren("LoginPath");

        public static string LogoutPath => _authConfigs.GetValueFromSectionChildren("LogoutPath");

        public static string RegisterPath => _authConfigs.GetValueFromSectionChildren("RegisterPath");

        public static double CookieExpiration => double.Parse(_authConfigs.GetValueFromSectionChildren("CookieExpiration"));

        public static string AuthenticationScheme => _authConfigs.GetValueFromSectionChildren("AuthenticationScheme");
    }
}
