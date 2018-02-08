using System.Threading.Tasks;

namespace Autyan.NiChiJou.Core.Data
{
    public interface IReporitory<TEntity, in TQuery, in TKey>
        where TQuery : BaseQuery<TKey>
        where TEntity : BaseEntity<TKey>
    {
        TEntity Insert(TEntity option);

        Task<TEntity> InsertAsync(TEntity option);

        TEntity Update(TEntity option);

        Task<TEntity> UpdateAsync(TEntity option);

        TEntity QueryById(TKey id);

        Task<TEntity> QueryByIdAsync(TKey id);

        PagedQueryResult<TEntity> QueryList(TQuery query);

        int DeleteById(object id);

        Task<int> DeleteByIdAsync(object id);
    }
}
