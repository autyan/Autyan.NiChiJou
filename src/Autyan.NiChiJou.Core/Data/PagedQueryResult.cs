using System.Collections.Generic;

namespace Autyan.NiChiJou.Core.Data
{
    public class PagedQueryResult<TEntity>
    {
        public IEnumerable<TEntity> Entities { get; set; }

        public int TotalCount { get; set; }
    }
}
