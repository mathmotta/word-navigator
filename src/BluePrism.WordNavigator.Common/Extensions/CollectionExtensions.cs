using System;
using System.Collections.Generic;

namespace BluePrism.WordNavigator.Common.Extensions
{
    /// <summary>
    /// Exponse extension methods for <see cref="ICollection{T}"/> types.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds a given item to the collection if the given <see cref="Func{T, TResult}"/> predicate against the given item is true.
        /// </summary>
        /// <typeparam name="T">The type of the collection.</typeparam>
        /// <param name="collection">The collection to be modified.</param>
        /// <param name="item">The item to be added to the collection.</param>
        /// <param name="predicate">The predicate to evaluate if the element should be added.</param>
        public static void AddIf<T>(this ICollection<T> collection, T item, Func<T, bool> predicate)
        {
            if (predicate(item))
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Adds a given item to the collection if the given boolean condition against the given item is true.
        /// </summary>
        /// <typeparam name="T">The type of the collection.</typeparam>
        /// <param name="collection">The collection to be modified.</param>
        /// <param name="item">The item to be added to the collection.</param>
        /// <param name="condition">The condition to evaluate if the element should be added.</param>
        public static void AddIf<T>(this ICollection<T> collection, T item, bool condition)
        {
            if (condition)
            {
                collection.Add(item);
            }
        }
    }
}
