using System.Collections.Generic;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Data;

namespace Autyan.NiChiJiu.Repository.Redis
{
    public abstract class RedisRepository<TEntity> : IRepository<TEntity>
    {
        public Task<TEntity> GetByIdAsyc(TEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<TEntity> FirstOrDefaultAsync(object query)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> QueryAsync(object query)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> DeleteByIdAsync(TEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> DeleteByConditionAsync(object condition)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> UpdateByIdAsync(TEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> UpdateByConditionAsync(object updateParamters, object condition)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedResult<TEntity>> PagingQueryAsync(IPagedQuery query)
        {
            throw new System.NotImplementedException();
        }

        public Task<long> InsertAsync(TEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> GetCountAsync(object condition)
        {
            throw new System.NotImplementedException();
        }
    }
}
