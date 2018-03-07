namespace Autyan.NiChiJou.Core.Utility.Sql
{
    public class MySqlBuilderFactory : ISqlBuilderFactory
    {
        public ISqlBuilder Start()
        {
            return MySqlBuilder.Start();
        }
    }
}
