using System;
using System.Linq;

namespace Autyan.NiChiJou.Core.Utility
{
    public static class UtilityHelper
    {
        public static string RandomString(Random random, int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
