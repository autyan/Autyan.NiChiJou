using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Data;
using Autyan.NiChiJou.Core.Extension;
using Autyan.NiChiJou.Core.Repository;
using Autyan.NiChiJou.Core.Utility.Sql;
using Dapper;

namespace Autyan.NiChiJou.Repository.Dapper
{
    public class BaseDapperRepository<TEntity> : BaseDapperRepository, IRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected DatabaseModelMetadata Metadata { get; }

        protected string TableName => Metadata.TableName;

        protected IEnumerable<string> Columns => Metadata.Columns;

        protected DatabaseGeneratedOption KeyOption => Metadata.Key.Option;

        protected BaseDapperRepository()
        {
            Metadata = MetadataContext.Instance[typeof(TEntity)];
        }

        public async Task<TEntity> GetByIdAsyc(TEntity entity)
        {
            return await FirstOrDefaultAsync(entity);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(object query)
        {
            var builder = BuildQuerySql(query);

            return await Connection.QueryFirstOrDefaultAsync<TEntity>(builder.End(), query);
        }

        public virtual async Task<IEnumerable<TEntity>> QueryAsync(object query)
        {
            var builder = BuildQuerySql(query);

            return await Connection.QueryAsync<TEntity>(builder.End(), query);
        }

        public virtual async Task<int> DeleteByIdAsync(TEntity entity)
        {
            var builder = StartSql();
            builder.Delete().FromTable(TableName).WhereAnd("Id = @Id");

            return await Connection.ExecuteAsync(builder.End(), entity);
        }

        public virtual async Task<int> DeleteByConditionAsync(object condition)
        {
            var builder = StartSql();
            builder.Delete().FromTable(TableName);
            AppendWhere(builder, condition);

            return await Connection.ExecuteAsync(builder.End(), condition);
        }

        public virtual async Task<int> UpdateByIdAsync(TEntity entity)
        {
            var builder = StartSql();
            builder.Update(TableName);
            foreach (var column in Columns.Where(c => c != "Id"))
            {
                builder.Set(column, $"@{column}");
            }

            builder.WhereAnd("Id = @Id");
            entity.ModifiedAt = DateTimeOffset.Now;

            return await Connection.ExecuteAsync(builder.End(), entity);
        }

        public virtual async Task<int> UpdateByConditionAsync(object updateParamters, object condition)
        {
            var dic = ParseUpdateValues(updateParamters);

            return await UpdateByConditionAsync(dic, condition);
        }

        protected async Task<int> UpdateByConditionAsync(Dictionary<string, object> updateParamters, object condition)
        {
            var builder = StartSql();
            builder.Update(TableName);
            foreach (var item in updateParamters)
            {
                builder.Set(item.Key, $"@{item.Key}");
            }
            AppendWhere(builder, condition, "w_");
            var whereConditions = GetObjectValues(condition, "w_");

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(updateParamters);
            parameters.AddDynamicParams(whereConditions);

            return await Connection.ExecuteAsync(builder.End(), parameters);
        }

        public virtual async Task<PagedResult<TEntity>> PagingQueryAsync(IPagedQuery query)
        {
            if (query.Take == null) throw new ArgumentNullException(nameof(query.Take));
            var queryBuilder = BuildQuerySql(query);
            queryBuilder.Skip(query.Skip).Take(query.Take);
            var results = await Connection.QueryAsync<TEntity>(queryBuilder.End(), query);

            var countBuilder = BuildCountSql(query);
            var count = await Connection.QueryAsync<int>(countBuilder.End(), query);

            return new PagedResult<TEntity>
            {
                Results = results,
                TotalCount = count.Single()
            };
        }

        public virtual async Task<long> InsertAsync(TEntity entity)
        {
            var builder = StartSql();
            entity.CreatedAt = DateTime.Now;
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
            return await Connection.ExecuteAsync(builder.End(), entity);
        }

        private void GetSequenceInsertSql(ISqlBuilder builder)
        {
            builder.InsertInto(TableName).Set("Id", "NEXT VALUE FOR DBO.EntityId");
            foreach (var column in Columns.Where(c => c != "Id").ToArray())
            {
                builder.Set(column, $"@{column}");
            }
        }

        private void GetIdentityInsertSql(ISqlBuilder builder)
        {
            var insertColumns = Columns.Where(c => c != "Id").ToArray();
            builder.InsertInto(TableName);
            foreach (var column in insertColumns)
            {
                builder.Set(column, $"@{column}");
            }
        }

        private void GetComputedInsertSql(ISqlBuilder builder)
        {
            var insertColumns = Columns.Where(c => c != "Id").ToArray();
            builder.InsertInto(TableName);
            foreach (var column in insertColumns)
            {
                builder.Set(column, $"@{column}");
            }
        }

        public virtual async Task<int> GetCountAsync(object condition)
        {
            var builder = StartSql();
            builder.SelectCount().FromTable(TableName);
            AppendWhere(builder, condition);
            var result = await Connection.QueryAsync<int>(builder.End(), condition);
            return result.Single();
        }

        protected virtual async Task<long> GetNextEntityIdAsync()
        {
            const string sql = "SELECT NEXT VALUE FOR EntityId FROM sys.sequences";
            return await Connection.ExecuteScalarAsync<long>(sql);
        }

        protected virtual ISqlBuilder BuildQuerySql(object query)
        {
            var builder = StartSql();
            builder.Select(string.Join(",", Columns)).FromTable(TableName);
            AppendWhere(builder, query);

            return builder;
        }

        protected virtual ISqlBuilder BuildCountSql(object query)
        {
            var builder = StartSql();
            builder.SelectCount().FromTable(TableName);
            AppendWhere(builder, query);

            return builder;
        }

        protected virtual void AppendWhere(ISqlBuilder builder, object whereCondition, string paramterPrefix = null)
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

        protected virtual Dictionary<string, object> ParseUpdateValues(object paramters)
        {
            var updateDic = new Dictionary<string, object>();
            foreach (var property in GetProperties(paramters.GetType()))
            {
                updateDic.Add(property.Name, property.GetValue(paramters));
            }

            return updateDic;
        }

        protected virtual void AppendWhereOnQueryParamter(ISqlBuilder builder, PropertyInfo queryParamter, string paramterPrefix = null)
        {
            if (paramterPrefix == null)
            {
                paramterPrefix = string.Empty;
            }
            if (queryParamter.Name.EndsWith("Range") && queryParamter.PropertyType.IsArray)
            {
                var fieldName = queryParamter.Name.RemoveTail("Range");
                builder.WhereAnd($"{fieldName} IN @{paramterPrefix}{queryParamter.Name}");
                return;
            }

            if (queryParamter.Name.EndsWith("From") && !queryParamter.PropertyType.IsArray)
            {
                var fieldName = queryParamter.Name.RemoveTail("From");
                builder.WhereAnd($"{fieldName} > @{paramterPrefix}{queryParamter.Name}");
                return;
            }

            if (queryParamter.Name.EndsWith("To") && !queryParamter.PropertyType.IsArray)
            {
                var fieldName = queryParamter.Name.RemoveTail("To");
                builder.WhereAnd($"{fieldName} < @{paramterPrefix}{queryParamter.Name}");
                return;
            }

            builder.WhereAnd($"{queryParamter.Name} = @{paramterPrefix}{queryParamter.Name}");
        }
    }

    public class BaseDapperRepository
    {
        private IDbConnection _connection;

        protected IDbConnection Connection => _connection ?? (_connection = DbConnectionFactory.GetConnection(DbConnectionName.Default));

        protected IDbConnectionFactory DbConnectionFactory => DapperConfiguration.Instance.DbConnectionFactory;

        protected ISqlBuilderFactory SqlBuilderFactory => DapperConfiguration.Instance.SqlBuilderFactory;

        protected ISqlBuilder StartSql()
        {
            return SqlBuilderFactory.Start();
        }

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
