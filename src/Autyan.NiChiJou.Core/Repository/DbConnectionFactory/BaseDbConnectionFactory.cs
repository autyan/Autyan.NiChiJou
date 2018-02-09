using System;
using Microsoft.Extensions.Configuration;

namespace Autyan.NiChiJou.Core.Repository.DbConnectionFactory
{
    public abstract class BaseDbConnectionFactory
    {
        public static IConfiguration Configuration { get; private set; }

        public static void SetConfigurationRoot(IConfiguration configuration)
        {
            if (Configuration != null) throw new InvalidOperationException("Confiuration is set");
            Configuration = configuration;
        }

        protected virtual string GetConnectionString(string name)
        {
            return Configuration.GetConnectionString(name);
        }
    }
}
