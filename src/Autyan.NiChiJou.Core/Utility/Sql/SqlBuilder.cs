using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Autyan.NiChiJou.Core.Utility.Sql
{
    public abstract class SqlBuilder : ISqlBuilder
    {
        protected Action? Action;

        protected string TableName;

        protected readonly IList<string> Columns = new List<string>();

        protected readonly IList<string> WhereClause = new List<string>();

        protected StringBuilder StrBuilder;

        protected readonly IList<string> OrderbyClause = new List<string>();

        protected readonly IList<KeyValuePair<string, string>> Values = new List<KeyValuePair<string, string>>();

        protected int? SkipRows;

        protected int? TakeRows;

        private void CheckAction()
        {
            if (Action != null) throw new InvalidOperationException("action set already.");
        }

        public ISqlBuilder InsertInto(string table)
        {
            CheckAction();
            Action = Sql.Action.Insert;
            TableName = table;
            return this;
        }

        public ISqlBuilder SelectCount()
        {
            CheckAction();
            Action = Sql.Action.Select;
            Columns.Add("COUNT(1)");
            return this;
        }

        public ISqlBuilder Count(string column)
        {
            Columns.Add($"COUNT({column})");
            return this;
        }

        public ISqlBuilder Select(string columns)
        {
            CheckAction();
            Action = Sql.Action.Select;
            Columns.Add(columns);
            return this;
        }

        public ISqlBuilder Update(string table)
        {
            CheckAction();
            Action = Sql.Action.Update;
            TableName = table;
            return this;
        }

        public ISqlBuilder Delete()
        {
            CheckAction();
            Action = Sql.Action.Delete;
            return this;
        }

        public ISqlBuilder FromTable(string tableName)
        {
            TableName = tableName;
            return this;
        }

        public ISqlBuilder Set(string key, string value)
        {
            Values.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        public ISqlBuilder WhereAnd(string condition)
        {
            WhereClause.Add($" AND {condition}");
            return this;
        }

        public ISqlBuilder WhereOr(string condition)
        {
            WhereClause.Add($" OR {condition}");
            return this;
        }

        public ISqlBuilder OrderBy(string column, bool isAsc = true)
        {
            OrderbyClause.Add(isAsc ? $" {column} " : $" {column} DESC");
            return this;
        }

        public ISqlBuilder Skip(int skip)
        {
            SkipRows = skip;
            return this;
        }

        public ISqlBuilder Take(int take)
        {
            TakeRows = take;
            return this;
        }

        public string End()
        {
            if (Action == null) throw new ArgumentNullException(nameof(Action));
            StrBuilder = new StringBuilder();
            BuildAction();
            StrBuilder.Append(";");

            return StrBuilder.ToString();
        }

        protected virtual void BuildAction()
        {
            switch (Action)
            {
                case Sql.Action.Insert:
                    BuildInsert();
                    break;
                case Sql.Action.Select:
                    BuildSelect();
                    break;
                case Sql.Action.Update:
                    BuildUpdate();
                    break;
                case Sql.Action.Delete:
                    BuildDelete();
                    break;
            }
        }

        protected virtual void BuildInsert()
        {
            StrBuilder.Append("INSERT INTO ").Append(TableName)
                .Append(" (").Append(string.Join(", ", Values.Select(v => v.Key)))
                .Append(") VALUES ")
                .Append(string.Join(", ", Values.Select(v => v.Value)));
            BuildWhere();
        }

        protected virtual void BuildSelect()
        {
            StrBuilder.Append("SELECT ").Append(string.Join(", ", Columns)).Append(" FROM ").Append(TableName);
            BuildWhere();
            if (OrderbyClause.Count == 0) return;

            StrBuilder.Append(" ORDER BY ");
            foreach (var description in OrderbyClause)
            {
                StrBuilder.Append(description);
            }

            if (TakeRows != null)
            {
                BuildTakeSkip();
            }
        }

        protected virtual void BuildUpdate()
        {
            StrBuilder.Append("UPDATE ").Append(TableName).Append(" SET ")
                .Append(string.Join(", ", Values.Select(v => $"{v.Key} = {v.Value}")));
            BuildWhere();
        }

        protected virtual void BuildDelete()
        {
            StrBuilder.Append("DELETE FROM ").Append(TableName);
            BuildWhere();
        }

        protected virtual void BuildWhere()
        {
            if (WhereClause.Count == 0) return;

            StrBuilder.Append(" WHERE (1=1) ");
            foreach (var condition in WhereClause)
            {
                StrBuilder.Append(condition);
            }
        }

        protected abstract void BuildTakeSkip();

        public override string ToString()
        {
            return End();
        }
    }

    public enum Action
    {
        Insert,

        Select,

        Update,

        Delete
    }
}
