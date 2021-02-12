using BluePrism.WordNavigator.Common.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BluePrism.WordNavigator.Common.Extensions
{
    public static class AsyncEnumerableExtensions
    {
        public static async Task<ConcurrentHashSet<TSource>> ToConcurrentHashSet<TSource>(this IAsyncEnumerable<TSource> source)
        {
            var hashSet = new ConcurrentHashSet<TSource>();
            await foreach (TSource element in source)
            {
                hashSet.Add(element);
            }
            return hashSet;
        }

        public static async Task<HashSet<TSource>> ToHashSet<TSource>(this IAsyncEnumerable<TSource> source)
        {
            var hashSet = new HashSet<TSource>();
            await foreach (TSource element in source)
            {
                hashSet.Add(element);
            }
            return hashSet;
        }
    }
}
