using System;

namespace Autyan.NiChiJou.Core.Data
{
    public class BaseQuery<TKey> : IPagedQuery
    {
        public TKey Id { get; set; }

        public TKey[] IdRange { get; set; }

        public TKey IdFrom { get; set; }

        public TKey IdTo { get; set; }

        public DateTime? CreatedAtFrom { get; set; }

        public DateTime? CreatedAtTo { get; set; }

        public DateTime? ModifiedAtFrom { get; set; }

        public DateTime? ModifiedAtTo { get; set; }

        public int? Take { get; set; }

        public int? Skip { get; set; }

        public string[] Asc { get; set; }

        public string[] Desc { get; set; }
    }
}
