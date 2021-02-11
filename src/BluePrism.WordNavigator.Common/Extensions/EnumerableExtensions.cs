using BluePrism.WordNavigator.Common.Concurrent;
using System;
using System.Collections.Generic;
using System.Text;

namespace BluePrism.WordNavigator.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static ConcurrentHashSet<TSource> ToConcurrentHashSet<TSource>(this IEnumerable<TSource> source)
        {
            var hashSet = new ConcurrentHashSet<TSource>();
            foreach (TSource element in source)
            {
                hashSet.Add(element);
            }
            return hashSet;
        }
    }
}
