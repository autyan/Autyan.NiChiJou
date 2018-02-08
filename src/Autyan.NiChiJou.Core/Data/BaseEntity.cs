using System;

namespace Autyan.NiChiJou.Core.Data
{
    public class BaseEntity<T> : BaseEntity, ICreateTrace<T>, IModifyTrace<T>
    {
        public virtual T Id { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public T CreatedBy { get; set; }

        public DateTimeOffset? ModifiedAt { get; set; }

        public T ModifiedBy { get; set; }
    }

    public class BaseEntity
    {
    }
}
