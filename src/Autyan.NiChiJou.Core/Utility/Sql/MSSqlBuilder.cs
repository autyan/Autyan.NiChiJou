using System.Linq;

namespace Autyan.NiChiJou.Core.Utility.Sql
{
    public class MsSqlBuilder : SqlBuilder
    {
        private MsSqlBuilder()
        {

        }

        public static ISqlBuilder Start() => new MsSqlBuilder();

        protected override void SequenceInsert()
        {
            StrBuilder.Append(" ( ").Append(KeyColumn).Append(", ").Append(string.Join(", ", Values.Select(v => v.Key)))
                .Append(" )").Append(" OUTPUT INSERTED.").Append(KeyColumn).Append(" ")
                .Append(" VALUES ( ").Append("NEXT VALUE FOR EntityId, ")
                .Append(string.Join(", ", Values.Where(v => !string.IsNullOrEmpty(v.Value)).Select(v => v.Value)))
                .Append(")");
        }

        protected override void ComputedInsert()
        {
            throw new System.NotImplementedException();
        }

        protected override void BuildTakeSkip()
        {
            StrBuilder.Append("OFFSET ").Append(SkipRows ?? 0).Append(" ROWS FETCH NEXT").Append(TakeRows)
                .Append(" ROWS ONLY ");
        }
    }
}
