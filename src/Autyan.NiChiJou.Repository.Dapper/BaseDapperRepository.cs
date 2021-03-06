﻿using System;
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

        protected DatabaseGeneratedOption KeyOption => Metadata.KeyOption;

        protected BaseDapperRepository(IDbConnectionFactory dbConnectionFactory,
            ISqlBuilderFactory sqlBuilderFactory) : base(dbConnectionFactory, sqlBuilderFactory)
        {
            Metadata = MetadataContext.Instance[typeof(TEntity)];
        }

        public async Task<TEntity> GetByIdAsync(TEntity entity)
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
            foreach (var column in Columns)
            {
                builder.Set(column, $"@{column}");
            }

            builder.WhereAnd("Id = @Id");
            entity.ModifiedAt = DateTime.Now;

            return await Connection.ExecuteAsync(builder.End(), entity);
        }

        public virtual async Task<int> PartialUpdateByIdAsync(TEntity entity)
        {
            var builder = StartSql();
            builder.Update(TableName);
            foreach (var updateProp in GetObjectValues(entity))
            {
                builder.Set(updateProp.Key, $"@{updateProp.Key}");
            }

            builder.WhereAnd("Id = @Id");
            entity.ModifiedAt = DateTime.Now;

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
            return await PagingQueryAsync<TEntity>(queryBuilder, query);
        }

        public virtual async Task<PagedResult<TSelect>> PagingQueryAsync<TSelect>(object columnObject, IPagedQuery query)
        {
            if (query.Take == null) throw new ArgumentNullException(nameof(query.Take));
            var columns = GetProperties(columnObject.GetType()).Select(p => p.Name).ToArray();
            return await PagingQueryAsync<TSelect>(columns, query);
        }

        public async Task<PagedResult<TSelect>> PagingQueryAsync<TSelect>(IEnumerable<string> columns, IPagedQuery query)
        {
            if (query.Take == null) throw new ArgumentNullException(nameof(query.Take));
            var queryBuilder = BuildDynamicQuerySql(columns, query);
            return await PagingQueryAsync<TSelect>(queryBuilder, query);
        }

        private async Task<PagedResult<TSelect>> PagingQueryAsync<TSelect>(ISqlBuilder queryBuilder, IPagedQuery query)
        {
            queryBuilder.Skip(query.Skip).Take(query.Take);
            var results = await Connection.QueryAsync<TSelect>(queryBuilder.End(), query);

            var countBuilder = BuildCountSql(query);
            var count = await Connection.QueryAsync<int>(countBuilder.End(), query);

            return new PagedResult<TSelect>
            {
                Results = results,
                TotalCount = count.Single()
            };
        }

        public virtual async Task<long> InsertAsync(TEntity entity)
        {
            var builder = StartSql();
            entity.CreatedAt = DateTime.Now;
            builder.InsertInto(TableName, KeyOption);
            foreach (var column in Columns)
            {
                builder.Set(column, $"@{column}");
            }
            return await Connection.ExecuteScalarAsync<long>(builder.End(), entity);
        }

        public virtual async Task<int> GetCountAsync(object condition)
        {
            var builder = StartSql();
            builder.SelectCount().FromTable(TableName);
            if (condition != null)
            {
                AppendWhere(builder, condition);
            }
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
            builder.Select($"{Metadata.Key}, {string.Join(",", Columns)}").FromTable(TableName);
            AppendWhere(builder, query);

            return builder;
        }

        protected virtual ISqlBuilder BuildDynamicQuerySql(IEnumerable<string> columns, object query)
        {
            var builder = StartSql();
            builder.Select(string.Join(",", columns)).FromTable(TableName);
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
                var propInfo = Metadata.Properties.FirstOrDefault(prop => prop.Name == property.Name);
                if (propInfo != null && propInfo.PropertyInfo.PropertyType == typeof(string))
                {
                    updateDic.Add(property.Name, new DbString { Value = property.GetValue(paramters).ToString(), Length = propInfo.MaxLength });
                }
                else
                {
                    updateDic.Add(property.Name, property.GetValue(paramters));
                }
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

        protected IDbConnectionFactory DbConnectionFactory { get; }

        protected ISqlBuilderFactory SqlBuilderFactory { get; }

        protected BaseDapperRepository(IDbConnectionFactory dbConnectionFactory,
            ISqlBuilderFactory sqlBuilderFactory)
        {
            DbConnectionFactory = dbConnectionFactory;
            SqlBuilderFactory = sqlBuilderFactory;
        }

        protected ISqlBuilder StartSql()
        {
            return SqlBuilderFactory.Start();
        }

        protected static ConcurrentDictionary<Type, List<PropertyInfo>> ParamtersCache { get; } = new ConcurrentDictionary<Type, List<PropertyInfo>>();

        protected static List<PropertyInfo> GetProperties(Type type)
        {
            if (ParamtersCache.ContainsKey(type)) return ParamtersCache[type];

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => !IgnorePropertyNames.Contains(p.Name.ToUpper())).ToList();
            ParamtersCache[type] = properties;

            return properties;
        }

        private static readonly string[] IgnorePropertyNames = new[] {"TAKE", "SKIP", "ASC", "DESC"};

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
