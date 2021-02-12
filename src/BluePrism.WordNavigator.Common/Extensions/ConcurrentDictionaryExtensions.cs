using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace BluePrism.WordNavigator.Common.Extensions
{
    public static class ConcurrentDictionaryExtensions
    {
        public static void TryAddIf<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue value, Func<TKey, TValue, bool> predicate)
        {
            if (predicate(key, value))
            {
                dictionary.TryAdd(key, value);
            }
        }

        public static void TryAddIf<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue value, bool condition)
        {
            if (condition)
            {
                dictionary.TryAdd(key, value);
            }
        }
    }
}
