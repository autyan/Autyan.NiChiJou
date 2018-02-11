using System;

namespace Autyan.NiChiJou.Core.Utility.Sql
{
    public class MySqlBuilder : SqlBuilder
    {
        private MySqlBuilder()
        {

        }

        public static ISqlBuilder Start() => new MySqlBuilder();

        protected override void BuildTakeSkip()
        {
            StrBuilder.Append("LIMIT ").Append(TakeRows).Append(" OFFSET ").Append(SkipRows ?? 0);
        }
    }
}
