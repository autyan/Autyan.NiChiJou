using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Autyan.NiChiJou.Repository.Dapper.Extension
{
    public static class DapperExtension
    {
        public static TEntity QueryById<TEntity>(this IDbConnection connection, object entity,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var sqlBuilder = new StringBuilder();
            var entityMetaData = GetMetadata(typeof(TEntity));
            var columns = entityMetaData.Columns.Where(c => c != "Id").ToArray();

            sqlBuilder.Append("SELECT ").Append(string.Join(", ", columns)).Append(" FROM ").Append(entityMetaData.TableName)
                .Append(" WHERE Id = @Id").Append(");");
            return connection.QueryFirstOrDefault<TEntity>(sqlBuilder.ToString(), entity, transaction, commandTimeout, commandType);
        }

        public static async Task<TEntity> QueryByIdAsync<TEntity>(this IDbConnection connection, object entity,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var sqlBuilder = new StringBuilder();
            var entityMetaData = GetMetadata(typeof(TEntity));
            var columns = entityMetaData.Columns.Where(c => c != "Id").ToArray();

            sqlBuilder.Append("SELECT ").Append(string.Join(", ", columns)).Append(" FROM ").Append(entityMetaData.TableName)
                .Append(" WHERE Id = @Id").Append(");");
            return await connection.QueryFirstOrDefaultAsync<TEntity>(sqlBuilder.ToString(), entity, transaction, commandTimeout, commandType);
        }

        public static TEntity Insert<TEntity>(this IDbConnection connection, object entity, DatabaseGeneratedOption? option = null,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var sqlBuilder = new StringBuilder();
            var entityMetaData = GetMetadata(typeof(TEntity));
            var columns = option == DatabaseGeneratedOption.None ? entityMetaData.Columns.ToArray() : entityMetaData.Columns.Where(c => c != "Id").ToArray();

            sqlBuilder.Append("INSERT INTO ").Append(entityMetaData.TableName).Append(" ")
                .Append(string.Join(", ", columns)).Append(" VALUES(").Append(string.Join(", ", columns.Select(c => $"@{c}"))).Append(");");
            return connection.ExecuteScalar<TEntity>(sqlBuilder.ToString(), entity, transaction, commandTimeout, commandType);
        }

        public static async Task<TEntity> InsertAsync<TEntity>(this IDbConnection connection, object entity, DatabaseGeneratedOption? option = null,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var sqlBuilder = new StringBuilder();
            var entityMetaData = GetMetadata(typeof(TEntity));
            var columns = option == DatabaseGeneratedOption.None ? entityMetaData.Columns.ToArray() : entityMetaData.Columns.Where(c => c != "Id").ToArray();

            sqlBuilder.Append("INSERT INTO ").Append(entityMetaData.TableName).Append(" ")
                .Append(string.Join(", ", columns)).Append(" VALUES(").Append(string.Join(", ", columns.Select(c => $"@{c}"))).Append(");");
            return await connection.ExecuteScalarAsync<TEntity>(sqlBuilder.ToString(), entity, transaction, commandTimeout, commandType);
        }

        public static int DeleteById<TEntity>(this IDbConnection connection, object entity,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var sqlBuilder = new StringBuilder();
            var entityMetaData = GetMetadata(typeof(TEntity));
            sqlBuilder.Append("DELETE FROM ").Append(entityMetaData.TableName).Append(" WHERE Id = @Id");
            return connection.Execute(sqlBuilder.ToString(), entity, transaction, commandTimeout, commandType);
        }

        public static async Task<int> DeleteByIdAsync<TEntity>(this IDbConnection connection, object entity,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var sqlBuilder = new StringBuilder();
            var entityMetaData = GetMetadata(typeof(TEntity));
            sqlBuilder.Append("DELETE FROM ").Append(entityMetaData.TableName).Append(" WHERE Id = @Id");
            return await connection.ExecuteAsync(sqlBuilder.ToString(), entity, transaction, commandTimeout, commandType);
        }

        public static TEntity UpdateById<TEntity>(this IDbConnection connection, object entity,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var sqlBuilder = new StringBuilder();
            var entityMetaData = GetMetadata(typeof(TEntity));
            var columns = entityMetaData.Columns.Where(c => c != "Id").ToArray();

            sqlBuilder.Append("UPDATE ").Append(entityMetaData.TableName).Append(" SET ");
            foreach (var column in columns)
            {
                sqlBuilder.Append($"{column} = @{column}");
            }

            sqlBuilder.Append(" WHERE Id = @Id");

            return connection.ExecuteScalar<TEntity>(sqlBuilder.ToString(), entity, transaction, commandTimeout, commandType);
        }

        public static async Task<TEntity> UpdateByIdAsync<TEntity>(this IDbConnection connection, object entity,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var sqlBuilder = new StringBuilder();
            var entityMetaData = GetMetadata(typeof(TEntity));
            var columns = entityMetaData.Columns.Where(c => c != "Id").ToArray();

            sqlBuilder.Append("UPDATE ").Append(entityMetaData.TableName).Append(" SET ");
            foreach (var column in columns)
            {
                sqlBuilder.Append($"{column} = @{column}");
            }

            sqlBuilder.Append(" WHERE Id = @Id");

            return await connection.ExecuteScalarAsync<TEntity>(sqlBuilder.ToString(), entity, transaction, commandTimeout, commandType);
        }

        public static IEnumerable<TEntity> QueryList<TEntity>(this IDbConnection connection, string sql, object query,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Query<TEntity>(sql, query, transaction, false, commandTimeout, commandType);
        }

        public static async Task<IEnumerable<TEntity>> QueryListAsync<TEntity>(this IDbConnection connection, string sql, object query,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await connection.QueryAsync<TEntity>(sql, query, transaction, commandTimeout, commandType);
        }

        private static MetaData GetMetadata(Type type)
        {
            return MetadataContext.Instance[type];
        }
    }
}
