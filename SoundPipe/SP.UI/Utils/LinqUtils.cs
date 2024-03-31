using System;
using System.Collections.Generic;

namespace SP.UI.Utils
{
    public static class LinqUtils
    {
        public static int FirstIndex<T>(this IEnumerable<T> sequence, Func<T, bool> predicate)
        {
            int i = 0;
            foreach (var item in sequence)
            {
                if (predicate(item))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }
    }
}
