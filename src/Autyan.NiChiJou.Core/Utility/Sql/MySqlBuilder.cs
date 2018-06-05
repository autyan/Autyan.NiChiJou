using System.Linq;

namespace Autyan.NiChiJou.Core.Utility.Sql
{
    public class MySqlBuilder : SqlBuilder
    {
        private MySqlBuilder()
        {

        }

        public static ISqlBuilder Start() => new MySqlBuilder();

        protected override void SequenceInsert()
        {
            StrBuilder.Append(" ( ").Append(KeyColumn).Append(", ").Append(string.Join(", ", Values.Select(v => v.Key)))
                .Append(" )").Append(" VALUES (").Append("next_value_for('entity_id'), ")
                .Append(string.Join(", ", Values.Where(v => !string.IsNullOrEmpty(v.Value)).Select(v => v.Value)))
                .Append(");\r\n").Append("SELECT current_value_for('entity_id')");
        }

        protected override void ComputedInsert()
        {
            throw new System.NotImplementedException();
        }

        protected override void BuildTakeSkip()
        {
            StrBuilder.Append("LIMIT ").Append(TakeRows).Append(" OFFSET ").Append(SkipRows ?? 0);
        }
    }
}
