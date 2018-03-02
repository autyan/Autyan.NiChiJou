using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autyan.NiChiJou.Core.Data
{
    public interface IRepository<TEntity>
    {
        Task<TEntity> GetByIdAsync(TEntity entity);

        Task<TEntity> FirstOrDefaultAsync(object query);

        Task<IEnumerable<TEntity>> QueryAsync(object query);

        Task<int> DeleteByIdAsync(TEntity entity);

        Task<int> DeleteByConditionAsync(object condition);

        Task<int> UpdateByIdAsync(TEntity entity);

        Task<int> UpdateByConditionAsync(object updateParamters, object condition);

        Task<PagedResult<TEntity>> PagingQueryAsync(IPagedQuery query);

        Task<PagedResult<TSelect>> PagingQueryAsync<TSelect>(object columnObject, IPagedQuery query);

        Task<PagedResult<TSelect>> PagingQueryAsync<TSelect>(IEnumerable<string> columns, IPagedQuery query);

        Task<long> InsertAsync(TEntity entity);

        Task<int> GetCountAsync(object condition);
    }
}
