using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Data;

namespace Autyan.NiChiJou.Repository.Dapper
{
    public class LongKeyDapperRepository<TEntity> : BaseDapperRepository<TEntity>
        where TEntity : LongKeyBaseEntity
    {
        public override async Task<long> InsertAsync(TEntity entity)
        {
            if (entity.CreatedBy == null)
            {
                entity.CreatedBy = -1;
            }
            return await base.InsertAsync(entity);
        }
    }
}
