using NUnit.Framework;
using System.Collections.Generic;
using BluePrism.WordNavigator.Common.Extensions;
using System.Threading.Tasks;

namespace BluePrism.WordNavigator.Common.Tests.Extensions
{
    public class EnumerableExtensionsTests
    {
        [Test]
        public void ToConcurrentHashSet_Success()
        {
            var enumerable = new List<int>() { 1 };
            var result = enumerable.ToConcurrentHashSet();

            Assert.IsTrue(result.Contains(1));
        }

        [Test]
        public async Task ToAsyncEnumerable_Success()
        {
            var enumerable = new List<int>() { 1 };
            var transformed = enumerable.ToAsyncEnumerable();

            bool result = false;
            await foreach (var item in transformed)
            {
                if(item == 1)
                {
                    result = true;
                    break;
                }
            }

            Assert.IsTrue(result);
        }
    }
}
