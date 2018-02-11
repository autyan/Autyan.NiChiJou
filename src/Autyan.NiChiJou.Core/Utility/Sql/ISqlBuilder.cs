namespace Autyan.NiChiJou.Core.Utility.Sql
{
    public interface ISqlBuilder
    {
        ISqlBuilder InsertInto(string table);

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

        string End();
    }
}
