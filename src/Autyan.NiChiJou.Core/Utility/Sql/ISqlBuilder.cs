using System.ComponentModel.DataAnnotations.Schema;

namespace Autyan.NiChiJou.Core.Utility.Sql
{
    public interface ISqlBuilder
    {
        ISqlBuilder SetKeyColumn(string keyColumn);

        ISqlBuilder InsertInto(string table, DatabaseGeneratedOption option = DatabaseGeneratedOption.None);

        ISqlBuilder SelectCount();

        ISqlBuilder Count(string column);

        ISqlBuilder Select(string columns);

        ISqlBuilder Update(string table);

        ISqlBuilder Delete();

        ISqlBuilder FromTable(string tableName);

        ISqlBuilder Set(string key, string value);

        ISqlBuilder WhereAnd(string condition);

        ISqlBuilder WhereOr(string condition);

        ISqlBuilder OrderBy(string column, bool isAsc = true);

        ISqlBuilder Skip(int? skip);

        ISqlBuilder Take(int? take);

        ISqlBuilder Output(string column);

        ISqlBuilder AppendSqlBuilder(ISqlBuilder builder);

        string End();
    }
}
