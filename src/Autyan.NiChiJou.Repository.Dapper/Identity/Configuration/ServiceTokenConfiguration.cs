using Autyan.NiChiJou.Model.Identity;

namespace Autyan.NiChiJou.Repository.Dapper.Identity.Configuration
{
    public class ServiceTokenConfiguration : DatabaseItemConfiguration<ServiceToken>
    {
        public ServiceTokenConfiguration()
        {
            this["ServiceName"].HasMaxLength(50);
            this["AppId"].HasMaxLength(50);
            this["ApiKey"].HasMaxLength(50);
        }
    }
}
