using System;
using System.Collections.Concurrent;

namespace BluePrism.WordNavigator.Common.Extensions
{
    /// <summary>
    /// Exposes extension methods to <see cref="ConcurrentDictionary{TKey, TValue}"/>
    /// </summary>
    public static class ConcurrentDictionaryExtensions
    {
        /// <summary>
        /// Adds a given key with a given value to the <see cref="ConcurrentDictionary"/> if the given predicates is evaluated true.
        /// </summary>
        /// <typeparam name="TKey">The keys' type.</typeparam>
        /// <typeparam name="TValue">The values' type</typeparam>
        /// <param name="dictionary">The dictionary to be modified.</param>
        /// <param name="key">The key to add</param>
        /// <param name="value">The value to assign to that key.</param>
        /// <param name="predicate">The predicate to be evaluated</param>
        public static void TryAddIf<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue value, Func<TKey, TValue, bool> predicate)
        {
            if (predicate(key, value))
            {
                dictionary.TryAdd(key, value);
            }
        }

        /// <summary>
        /// Adds a given key with a given value to the <see cref="ConcurrentDictionary"/> if the given predicates is evaluated true.
        /// </summary>
        /// <typeparam name="TKey">The keys' type.</typeparam>
        /// <typeparam name="TValue">The values' type</typeparam>
        /// <param name="dictionary">The dictionary to be modified.</param>
        /// <param name="key">The key to add</param>
        /// <param name="value">The value to assign to that key.</param>
        /// <param name="condition">The condition to be evaluated</param>
        public static void TryAddIf<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue value, bool condition)
        {
            if (condition)
            {
                dictionary.TryAdd(key, value);
            }
        }
    }
}
