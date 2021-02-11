using BluePrism.WordNavigator.Common.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BluePrism.WordNavigator.Common.Tests.Extensions
{
    public class AsyncEnumerableExtensionsTests
    {
       [Test]
       public async Task ToConcurrentHashSet_Success()
        {
            var list = new List<int>() { 1 };
            var asyncEnumerable = list.ToAsyncEnumerable();

            var concurrentHashSet = await asyncEnumerable.ToConcurrentHashSet();
            Assert.IsTrue(concurrentHashSet.Contains(1));
        }

        [Test]
        public async Task ToHashSet_Success()
        {
            var list = new List<int>() { 1 };
            var asyncEnumerable = list.ToAsyncEnumerable();

            var hashSet = await asyncEnumerable.ToHashSet();
            Assert.IsTrue(hashSet.Contains(1));
        }
    }
}
