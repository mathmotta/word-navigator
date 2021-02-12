using BluePrism.WordNavigator.Common.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BluePrism.WordNavigator.Common.Extensions
{
    /// <summary>
    /// Expose extension methods for <see cref="IAsyncEnumerable{T}"/>'s
    /// </summary>
    public static class AsyncEnumerableExtensions
    {
        /// <summary>
        /// Transforms the <see cref="IAsyncEnumerable{T}"/> into a <see cref="ConcurrentHashSet{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The generic type the enumration uses.</typeparam>
        /// <param name="source">The <see cref="IAsyncEnumerable{T}"/> to be transformed.</param>
        /// <returns>The <see cref="ConcurrentHashSet{T}"/> object.</returns>
        public static async Task<ConcurrentHashSet<TSource>> ToConcurrentHashSet<TSource>(this IAsyncEnumerable<TSource> source)
        {
            var hashSet = new ConcurrentHashSet<TSource>();
            await foreach (TSource element in source)
            {
                hashSet.Add(element);
            }
            return hashSet;
        }

        /// <summary>
        /// Transforms the <see cref="IAsyncEnumerable{T}"/> into a <see cref="HashSet{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The generic type the enumration uses.</typeparam>
        /// <param name="source">The <see cref="IAsyncEnumerable{T}"/> to be transformed.</param>
        /// <returns>The <see cref="HashSet{T}"/> object.</returns>
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
