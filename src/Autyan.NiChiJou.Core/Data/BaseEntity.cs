using System;

namespace Autyan.NiChiJou.Core.Data
{
    public class BaseEntity<TKey, TUserKey> : BaseEntity, ICreateTrace<TUserKey>, IModifyTrace<TUserKey>
    {
        public virtual TKey Id { get; set; }

        public TUserKey CreatedBy { get; set; }

        public TUserKey ModifiedBy { get; set; }
    }

    public class BaseEntity
    {
        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
