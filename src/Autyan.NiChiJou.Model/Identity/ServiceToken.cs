using Autyan.NiChiJou.Core.Data;

namespace Autyan.NiChiJou.Model.Identity
{
    public class ServiceToken : LongKeyBaseEntity
    {
        public virtual string ServiceName { get; set; }

        public virtual string AppId { get; set; }

        public virtual string ApiKey { get; set; }

        public virtual bool? IsEnabled { get; set; }
    }

    public class ServiceTokenQuery : LongKeyBaseEntityQuery
    {
        public string ServiceName { get; set; }

        public string AppId { get; set; }

        public bool? IsEnabled { get; set; }
    }
}
