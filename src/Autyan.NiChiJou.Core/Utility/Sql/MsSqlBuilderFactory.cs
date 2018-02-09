namespace Autyan.NiChiJou.Core.Utility.Sql
{
    public class MsSqlBuilderFactory : ISqlBuilderFactory
    {
        public ISqlBuilder Start()
        {
            return MsSqlBuilder.Start();
        }
    }
}
