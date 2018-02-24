using System;
using Autyan.NiChiJou.Core.Utility;

namespace Autyan.NiChiJou.Core.Extension
{
    public static class RandomExtension
    {
        public static string RandomString(this Random random, int length)
        {
            return UtilityHelper.RandomString(random, length);
        }
    }
}
