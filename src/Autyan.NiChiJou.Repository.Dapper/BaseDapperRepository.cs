using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Data;
using Autyan.NiChiJou.Core.Extension;
using Autyan.NiChiJou.Core.Repository;
using Dapper;

namespace Autyan.NiChiJou.Repository.Dapper
{
    public class BaseDapperRepository<TEntity> : BaseDapperRepository, IRepository<TEntity>
        where TEntity : BaseEntity
    {
        private IDbConnection _connection;

        protected IDbConnection Connection => _connection ?? (_connection = Factory.GetConnection(DbConnectionName.Default));

        protected readonly IDbConnectionFactory Factory;

        protected DatabaseModelMetadata Metadata { get; }

        protected string TableName => Metadata.TableName;

        protected IEnumerable<string> Columns => Metadata.Columns;

        protected DatabaseGeneratedOption KeyOption => Metadata.Key.Option;

        protected BaseDapperRepository()
        {
            Factory = new DefaultDbConnectionFactory();
            Metadata = MetadataContext.Instance[typeof(TEntity)];
        }

        public async Task<TEntity> GetByIdAsyc(TEntity entity)
        {
            return await FirstOrDefaultAsync(entity);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(object query)
        {
            var builder = BuildQuerySql(query);

            return await Connection.QueryFirstOrDefaultAsync<TEntity>(builder.ToString(), query);
        }

        public virtual async Task<IEnumerable<TEntity>> QueryAsync(object query)
        {
            var builder = BuildQuerySql(query);

            return await Connection.QueryAsync<TEntity>(builder.ToString(), query);
        }

        public virtual async Task<int> DeleteByIdAsync(TEntity entity)
        {
            var builder = new StringBuilder();
            builder.Append("DELETE FROM ").Append(TableName).Append(" WHERE Id = @Id");

            return await Connection.ExecuteAsync(builder.ToString(), entity);
        }

        public virtual async Task<int> DeleteByConditionAsync(object condition)
        {
            var builder = new StringBuilder();
            builder.Append("DELETE FROM ").Append(TableName).Append(" WHERE 1 = 1");
            AppendWhere(builder, condition);

            return await Connection.ExecuteAsync(builder.ToString(), condition);
        }

        public virtual async Task<int> UpdateByIdAsync(TEntity entity)
        {
            var builder = new StringBuilder();
            builder.Append("UPDATE ").Append(TableName).Append(" SET ");
            builder.Append(string.Join(",", Columns.Where(c => c != "Id").Select(c => $"{c} = @{c}")));
            builder.Append(" WHERE Id = @Id");
            entity.ModifiedAt = DateTime.Now;

            return await Connection.ExecuteAsync(builder.ToString(), entity);
        }

        public virtual async Task<int> UpdateByConditionAsync(object updateParamters, object condition)
        {
            var builder = new StringBuilder();
            builder.Append("UPDATE ").Append(TableName).Append(" SET ");
            var updateProperties = GetProperties(updateParamters.GetType());
            builder.Append(string.Join(",", updateProperties.Select(c => $"{c.Name} = @{c.Name}")));
            builder.Append(" WHERE 1 = 1");
            AppendWhere(builder, condition, "w_");
            var whereConditions = GetObjectValues(condition, "w_");

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(updateParamters);
            parameters.AddDynamicParams(whereConditions);

            return await Connection.ExecuteAsync(builder.ToString(), parameters);
        }

        public virtual async Task<PagedResult<TEntity>> PagingQueryAsync(IPagedQuery query)
        {
            if (query.Take == null) throw new ArgumentNullException(nameof(query.Take));
            var queryBuilder = BuildQuerySql(query);
            queryBuilder.Append("OFFSET ").Append(query.Skip ?? 0).Append(" ROWS FETCH NEXT ").Append(query.Take)
                .Append(" ROWS ONLY");
            var results = await Connection.QueryAsync<TEntity>(queryBuilder.ToString(), query);

            var countBuilder = BuildCountSql(query);
            var count = await Connection.QueryAsync<int>(countBuilder.ToString(), query);

            return new PagedResult<TEntity>
            {
                Results = results,
                TotalCount = count.Single()
            };
        }

        public virtual async Task<long> InsertAsync(TEntity entity)
        {
            var builder = new StringBuilder();
            switch (KeyOption)
            {
                case DatabaseGeneratedOption.None:
                    GetSequenceInsertSql(builder);
                    break;
                case DatabaseGeneratedOption.Identity:
                    GetIdentityInsertSql(builder);
                    break;
                case DatabaseGeneratedOption.Computed:
                    GetComputedInsertSql(builder);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            entity.CreatedAt = DateTime.Now;
            return await Connection.ExecuteAsync(builder.ToString(), entity);
        }

        private void GetSequenceInsertSql(StringBuilder builder)
        {
            builder.Append("INSERT INTO ").Append(TableName).Append(" ( ")
                .Append(string.Join(", ", Columns)).Append(" ) VALUES (")
                .Append(" NEXT VALUE FOR DBO.EntityId, ")
                .Append(string.Join(", ", Columns.Where(c => c != "Id").Select(c => $"@{c}")))
                .Append(" )");
        }

        private void GetIdentityInsertSql(StringBuilder builder)
        {
            var insertColumns = Columns.Where(c => c != "Id").ToArray();
            builder.Append("INSERT INTO ").Append(TableName).Append(" ( ")
                .Append(string.Join(", ", insertColumns)).Append(" ) VALUES (")
                .Append(string.Join(", ", insertColumns.Select(c => $"@{c}")))
                .Append(" )");
        }

        private void GetComputedInsertSql(StringBuilder builder)
        {
            var insertColumns = Columns.Where(c => c != "Id").ToArray();
            builder.Append("INSERT INTO ").Append(TableName).Append(" ( ")
                .Append(string.Join(", ", insertColumns)).Append(" ) VALUES (")
                .Append(string.Join(", ", insertColumns.Select(c => $"@{c}")))
                .Append(" )");
        }

        public virtual async Task<int> GetCountAsync(object condition)
        {
            var builder = new StringBuilder();
            builder.Append("SELECT COUNT(1) FROM ").Append(TableName).Append(" WHERE 1= 1");
            AppendWhere(builder, condition);
            var result = await Connection.QueryAsync<int>(builder.ToString(), condition);
            return result.Single();
        }

        protected virtual async Task<long> GetNextEntityIdAsync()
        {
            const string sql = "SELECT NEXT VALUE FOR EntityId FROM sys.sequences";
            return await Connection.ExecuteScalarAsync<long>(sql);
        }

        protected virtual StringBuilder BuildQuerySql(object query)
        {
            var builder = new StringBuilder();
            builder.Append("SELECT ").Append(string.Join(",", Columns)).Append(" FROM ").Append(TableName).Append(" WHERE 1= 1");
            AppendWhere(builder, query);
            builder.Append(";");

            return builder;
        }

        protected virtual StringBuilder BuildCountSql(object query)
        {
            var builder = new StringBuilder();
            builder.Append("SELECT COUNT(1) FROM ").Append(TableName).Append(" WHERE 1= 1");
            AppendWhere(builder, query);

            return builder;
        }

        protected virtual void AppendWhere(StringBuilder builder, object whereCondition, string paramterPrefix = null)
        {
            var queryParamters = GetProperties(whereCondition.GetType())
                .Where(p => p.PropertyType.IsQueryTypes());

            foreach (var queryParamter in queryParamters)
            {
                if (queryParamter.GetValue(whereCondition) != null)
                {
                    AppendWhereOnQueryParamter(builder, queryParamter, paramterPrefix);
                }
            }
        }

        protected virtual void AppendWhereOnQueryParamter(StringBuilder builder, PropertyInfo queryParamter, string paramterPrefix = null)
        {
            if (paramterPrefix == null)
            {
                paramterPrefix = string.Empty;
            }
            if (queryParamter.Name.EndsWith("Range") && queryParamter.PropertyType.IsArray)
            {
                var fieldName = queryParamter.Name.RemoveTail("Range");
                builder.Append(" AND ").Append(fieldName).Append(" IN @").Append(paramterPrefix).Append(queryParamter.Name);
                return;
            }

            if (queryParamter.Name.EndsWith("From") && !queryParamter.PropertyType.IsArray)
            {
                var fieldName = queryParamter.Name.RemoveTail("From");
                builder.Append(" AND ").Append(fieldName).Append(" > @").Append(paramterPrefix).Append(queryParamter.Name);
                return;
            }

            if (queryParamter.Name.EndsWith("To") && !queryParamter.PropertyType.IsArray)
            {
                var fieldName = queryParamter.Name.RemoveTail("To");
                builder.Append(" AND ").Append(fieldName).Append(" < @").Append(paramterPrefix).Append(queryParamter.Name);
                return;
            }

            builder.Append(" AND ").Append(queryParamter.Name).Append(" = @").Append(queryParamter.Name);
        }
    }

    public class BaseDapperRepository
    {
        protected static ConcurrentDictionary<Type, List<PropertyInfo>> ParamtersCache { get; } = new ConcurrentDictionary<Type, List<PropertyInfo>>();

        protected static List<PropertyInfo> GetProperties(Type type)
        {
            if (ParamtersCache.ContainsKey(type)) return ParamtersCache[type];

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            ParamtersCache[type] = properties;

            return properties;
        }

        protected static IDictionary<string, object> GetObjectValues(object obj, string keyPrefix = null, bool ignoreNullValues = true)
        {
            var dic = new Dictionary<string, object>();
            if (obj == null)
            {
                return dic;
            }

            if (keyPrefix == null)
            {
                keyPrefix = string.Empty;
            }
            foreach (var property in GetProperties(obj.GetType()))
            {
                var value = property.GetValue(obj);
                if (ignoreNullValues && value == null)
                {
                    //ignore
                }
                else
                {
                    dic.Add(keyPrefix + property.Name, value);
                }
            }
            return dic;
        }
    }
}
