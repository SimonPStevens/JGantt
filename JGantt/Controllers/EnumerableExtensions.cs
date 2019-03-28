using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Linq
{
    public static class EnumerableExtensions {
        public static TResult MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult defaultValue)
        {
            return source.Any() ? source.Max(selector) : defaultValue;
        }
    }
}
