using System.Collections.Generic;

namespace Autyan.NiChiJou.Core.Data
{
    public class PagedResult<TEntity>
    {
        public IEnumerable<TEntity> Results { get; set; }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }
    }
}
