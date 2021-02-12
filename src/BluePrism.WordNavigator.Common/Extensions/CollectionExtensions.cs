using System;
using System.Collections.Generic;

namespace BluePrism.WordNavigator.Common.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddIf<T>(this ICollection<T> collection, T item, Func<T, bool> predicate)
        {
            if (predicate(item))
            {
                collection.Add(item);
            }
        }

        public static void AddIf<T>(this ICollection<T> collection, T item, bool condition)
        {
            if (condition)
            {
                collection.Add(item);
            }
        }
    }
}
