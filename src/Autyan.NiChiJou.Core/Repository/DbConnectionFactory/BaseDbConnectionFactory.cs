using Autyan.NiChiJou.Core.Config;

namespace Autyan.NiChiJou.Core.Repository.DbConnectionFactory
{
    public abstract class BaseDbConnectionFactory
    {
        protected virtual string GetConnectionString(string name)
        {
            return ResourceConfiguration.ConnectionStrings(name);
        }
    }
}
