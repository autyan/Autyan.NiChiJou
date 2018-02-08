using System;

namespace Autyan.NiChiJou.Core.Data
{
    public class LongKeyBaseEntity : BaseEntity<long?>
    {

    }

    public class LongKeyBaseEntityQuery : BaseQuery<long?>
    {
        public DateTimeOffset? CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTimeOffset? ModifiedAt { get; set; }

        public long? ModifiedBy { get; set; }
    }
}
