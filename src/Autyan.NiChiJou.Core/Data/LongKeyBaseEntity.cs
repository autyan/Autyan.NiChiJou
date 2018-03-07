using System;

namespace Autyan.NiChiJou.Core.Data
{
    public abstract class LongKeyBaseEntity : BaseEntity<long?, long?>
    {
    }

    public class LongKeyBaseEntityQuery : BaseQuery<long?>
    {
        public DateTime? CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public long? ModifiedBy { get; set; }
    }
}
