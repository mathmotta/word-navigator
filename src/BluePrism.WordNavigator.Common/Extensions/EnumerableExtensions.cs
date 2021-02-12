using BluePrism.WordNavigator.Common.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public static async IAsyncEnumerable<TSource> ToAsyncEnumerable<TSource>(this IEnumerable<TSource> source)
        {
            foreach (var element in source)
            {
                yield return await Task.FromResult(element);
            }
        }
    }
}
