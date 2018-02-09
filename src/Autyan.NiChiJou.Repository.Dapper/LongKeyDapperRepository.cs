using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Data;
using Autyan.NiChiJou.Core.Utility.Sql;

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

        public override async Task<int> UpdateByIdAsync(TEntity entity)
        {
            if (entity.ModifiedBy == null)
            {
                entity.ModifiedBy = -1;
            }
            return await base.UpdateByIdAsync(entity);
        }

        protected override void SetUpdateValue(ISqlBuilder builder, object updateParamters)
        {
            builder.Set("ModifiedBy", "@ModifiedBy");
            base.SetUpdateValue(builder, updateParamters);
        }
    }
}
