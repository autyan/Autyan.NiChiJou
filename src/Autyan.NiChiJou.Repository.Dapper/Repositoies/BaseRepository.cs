using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Data;
using Autyan.NiChiJou.Core.Utility;
using Autyan.NiChiJou.Repository.Dapper.Extension;

namespace Autyan.NiChiJou.Repository.Dapper.Repositoies
{
    public class BaseRepository<TEntity, TQuery, TKey> : IReporitory<TEntity, TQuery, TKey>
        where TQuery : BaseQuery<TKey>
        where TEntity : BaseEntity<TKey>
    {
        protected readonly IDbConnection Connection;

        public BaseRepository(IDbConnection connection)
        {
            Connection = connection;
        }

        public TEntity QueryById(TKey id)
        {
            return Connection.QueryById<TEntity>(new {Id = id});
        }

        public async Task<TEntity> QueryByIdAsync(TKey id)
        {
            return await Connection.QueryByIdAsync<TEntity>(new { Id = id });
        }

        public PagedQueryResult<TEntity> QueryList(TQuery query)
        {
            throw new System.NotImplementedException();
        }

        public virtual int DeleteById(object id)
        {
            return Connection.DeleteById<TEntity>(new {Id = id});
        }

        public virtual async Task<int> DeleteByIdAsync(object id)
        {
            return await Connection.DeleteByIdAsync<TEntity>(new { Id = id });
        }

        public virtual TEntity Insert(TEntity entity)
        {
            return Insert(entity, DatabaseGeneratedOption.None);
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            return await InsertAsync(entity, DatabaseGeneratedOption.None);
        }

        public virtual TEntity Update(TEntity option)
        {
            return Connection.UpdateById<TEntity>(option);
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity option)
        {
            return await Connection.UpdateByIdAsync<TEntity>(option);
        }

        protected virtual TEntity Insert(TEntity entity, DatabaseGeneratedOption option,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.Insert<TEntity>(entity, option, transaction, commandTimeout, commandType);
        }

        protected virtual async Task<TEntity> InsertAsync(TEntity entity, DatabaseGeneratedOption option,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await Connection.InsertAsync<TEntity>(entity, option, transaction, commandTimeout, commandType);
        }

        protected virtual string GetQuerySql<T>(TQuery query)
        {
            var metadata = MetadataContext.Instance[typeof(T)];
            var builder = SqlBuilder.Begin();
            builder.Select(string.Join(", ", metadata.Columns)).FromTable(metadata.TableName);
            AppendWhere(builder, query);

            return builder.End();
        }

        protected virtual void AppendWhere(SqlBuilder builder, TQuery query)
        {
            if (query.Id != null)
            {
                builder.WhereAnd(" Id = @Id ");
            }

            if (query.Ids != null && query.Ids.Length > 0)
            {
                builder.WhereAnd(" Id IN @Ids ");
            }

            if (query.IdFrom != null)
            {
                builder.WhereAnd(" Id > @IdFrom ");
            }

            if (query.IdTo != null)
            {
                builder.WhereAnd(" Id < @IdTo ");
            }

            if (query.CreatedAtFrom != null)
            {
                builder.WhereAnd(" CreatedAt > @CreatedAtFrom ");
            }

            if (query.CreatedAtTo != null)
            {
                builder.WhereAnd(" CreatedAt > @CreatedAtTo ");
            }

            if (query.ModifiedAtFrom != null)
            {
                builder.WhereAnd(" CreatedAt > @ModifiedAtFrom ");
            }

            if (query.ModifiedAtTo != null)
            {
                builder.WhereAnd(" CreatedAt > @ModifiedAtTo ");
            }
        }
    }
}
