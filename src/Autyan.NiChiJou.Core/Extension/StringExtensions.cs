using System;

namespace Autyan.NiChiJou.Core.Extension
{
    public static class StringExtensions
    {
        public static string RemoveTail(this string original, string removeWord, StringComparison comparison = StringComparison.Ordinal)
        {
            if (!original.EndsWith(removeWord)) return original;

            var wordIndex = original.IndexOf(removeWord, comparison);
            return original.Remove(wordIndex, removeWord.Length);
        }
    }
}
