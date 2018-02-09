namespace Autyan.NiChiJou.Core.Utility.Sql
{
    public class MsSqlBuilder : SqlBuilder
    {
        protected override void BuildTakeSkip()
        {
            StrBuilder.Append("OFFSET ").Append(SkipRows ?? 0).Append(" ROWS FETCH NEXT").Append(TakeRows)
                .Append(" ROWS ONLY ");
        }
    }
}
