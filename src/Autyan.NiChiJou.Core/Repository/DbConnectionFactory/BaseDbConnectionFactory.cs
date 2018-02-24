using Autyan.NiChiJou.Core.Config;
using Microsoft.Extensions.Configuration;

namespace Autyan.NiChiJou.Core.Repository.DbConnectionFactory
{
    public abstract class BaseDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        protected BaseDbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected virtual string GetConnectionString(string name)
        {
            return _configuration.GetConnectionString(name);
        }
    }
}
