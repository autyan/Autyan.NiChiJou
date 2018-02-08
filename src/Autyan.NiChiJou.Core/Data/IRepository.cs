using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autyan.NiChiJou.Core.Data
{
    public interface IRepository<TEntity, in TKey>
        where TEntity : BaseEntity<TKey>
    {
        Task<TEntity> GetByIdAsyc(TKey id);

        Task<TEntity> FirstOrDefaultAsync(object query);

        Task<IEnumerable<TEntity>> QueryAsync(object query);

        Task<int> DeleteByIdAsync(TEntity entity);

        Task<int> DeleteByConditionAsync(object condition);

        Task<int> UpdateByIdAsync(TEntity entity);

        Task<int> UpdateByConditionAsync(object updateParamters, object condition);

        Task<PagedResult<TEntity>> PagingQueryAsync(IPagedQuery query);

        Task<long> InsertAsync(TEntity entity);

        Task<int> GetCountAsync(object condition);
    }
}
