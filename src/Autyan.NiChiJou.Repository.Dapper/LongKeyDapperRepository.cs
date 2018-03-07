using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Data;
using Autyan.NiChiJou.Core.Repository;
using Autyan.NiChiJou.Core.Utility.Sql;

namespace Autyan.NiChiJou.Repository.Dapper
{
    public class LongKeyDapperRepository<TEntity> : BaseDapperRepository<TEntity>
        where TEntity : LongKeyBaseEntity
    {
        protected LongKeyDapperRepository(IDbConnectionFactory dbConnectionFactory, ISqlBuilderFactory sqlBuilderFactory) : base(dbConnectionFactory, sqlBuilderFactory)
        {
        }

        public override async Task<long> InsertAsync(TEntity entity)
        {
            if (entity.CreatedBy == null)
            {
                entity.CreatedBy = -1;
            }
            var insertedId = await base.InsertAsync(entity);
            entity.Id = insertedId;
            return insertedId;
        }

        public override async Task<int> UpdateByIdAsync(TEntity entity)
        {
            if (entity.ModifiedBy == null)
            {
                entity.ModifiedBy = -1;
            }
            return await base.UpdateByIdAsync(entity);
        }

        protected override Dictionary<string, object> ParseUpdateValues(object paramters)
        {
            var dic = base.ParseUpdateValues(paramters);
            dic.Add("ModifiedAt", DateTime.Now);
            dic.Add("ModifiedBy", -1);
            return dic;
        }
    }
}
