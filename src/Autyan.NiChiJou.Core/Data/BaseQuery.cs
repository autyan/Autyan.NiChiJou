using System;

namespace Autyan.NiChiJou.Core.Data
{
    public class BaseQuery<TKey> : IPagedQuery
    {
        public TKey Id { get; set; }

        public TKey[] IdRange { get; set; }

        public TKey IdFrom { get; set; }

        public TKey IdTo { get; set; }

        public DateTimeOffset? CreatedAtFrom { get; set; }

        public DateTimeOffset? CreatedAtTo { get; set; }

        public DateTimeOffset? ModifiedAtFrom { get; set; }

        public DateTimeOffset? ModifiedAtTo { get; set; }

        public int? Take { get; set; }

        public int? Skip { get; set; }
    }
}
