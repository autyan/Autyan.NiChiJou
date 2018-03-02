using System.Collections.Generic;

namespace Autyan.NiChiJou.Core.Data
{
    public class PagedResult<TEntity>
    {
        public IEnumerable<TEntity> Results { get; set; }

        public int TotalCount { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }
    }
}
