using Autyan.NiChiJou.Core.Data;
using Autyan.NiChiJou.Model.Enumeration;

namespace Autyan.NiChiJou.Model.Blog
{
    public class ActivityLog : LongKeyBaseEntity
    {
        public virtual long? OperateUserId { get; set; }

        public virtual ActivityType ActivityType { get; set; }

        public virtual string Content { get; set; }

        public virtual string OperateIpAddress { get; set; }

        public virtual ClientType ClientType { get; set; }
    }

    public class ActivityLogQuery : LongKeyBaseEntityQuery
    {
        public long? OperateUserId { get; set; }

        public ActivityType ActivityType { get; set; }

        public string Content { get; set; }

        public string OperateIpAddress { get; set; }

        public ClientType ClientType { get; set; }
    }
}
