using System.Collections.Generic;
using Autyan.NiChiJou.Core.Extension;
using Microsoft.Extensions.Configuration;

namespace Autyan.NiChiJou.Core.Config
{
    public static class ResourceConfiguration
    {
        private static IConfiguration _configuration;

        private static IEnumerable<IConfigurationSection> _redisConfigs;

        private static IEnumerable<IConfigurationSection> _sessionConfigs;

        private static IEnumerable<IConfigurationSection> _cookieAuthConfigs;

        private static IEnumerable<IConfigurationSection> _serviceTokenAuthConfigs;

        private static IEnumerable<IConfigurationSection> _authConfigs;

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
            _cookieAuthConfigs = _configuration.GetSection("Auth").GetSection("Cookie").GetChildren();
            _serviceTokenAuthConfigs = _configuration.GetSection("Auth").GetSection("ServiceToken").GetChildren();
        }

        public static string RedisAddress => _redisConfigs.GetValueFromSectionChildren("Address");

        public static string RedisInstanceName => _redisConfigs.GetValueFromSectionChildren("InstanceName");

        public static double RedisDefaultExpiration => double.Parse(_redisConfigs.GetValueFromSectionChildren("DefaultExpiration"));

        public static double SessionExpiration => double.Parse(_sessionConfigs.GetValueFromSectionChildren("Expiration"));

        public static string ConnectionStrings(string name) => _configuration.GetConnectionString(name);

        public static string LoginPath => _cookieAuthConfigs.GetValueFromSectionChildren("LoginPath");

        public static string LogoutPath => _cookieAuthConfigs.GetValueFromSectionChildren("LogoutPath");

        public static string RegisterPath => _cookieAuthConfigs.GetValueFromSectionChildren("RegisterPath");

        public static double CookieExpiration => double.Parse(_cookieAuthConfigs.GetValueFromSectionChildren("CookieExpiration"));

        public static string CookieAuthenticationScheme => _cookieAuthConfigs.GetValueFromSectionChildren("Scheme");

        public static string ServiceTokenAuthenticationScheme => _serviceTokenAuthConfigs.GetValueFromSectionChildren("Scheme");

        public static string ServiceTokenAppId => _serviceTokenAuthConfigs.GetValueFromSectionChildren("AppId");

        public static string ServiceTokenApiKey => _serviceTokenAuthConfigs.GetValueFromSectionChildren("ApiKey");

        public static ulong ServiceTokenMaxAge =>
            ulong.Parse(_serviceTokenAuthConfigs.GetValueFromSectionChildren("MaxAge"));

        public static string UnifyLoginServer => _authConfigs.GetValueFromSectionChildren(nameof(UnifyLoginServer));
    }
}
