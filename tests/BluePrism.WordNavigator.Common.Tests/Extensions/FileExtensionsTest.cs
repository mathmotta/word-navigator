using BluePrism.WordNavigator.Common.Extensions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace BluePrism.WordNavigator.Common.Tests.Extensions
{
    public class FileExtensionsTest
    {
        [Test]
        public async Task ReadLinesAsync_Success()
        {
            var lines = FileExtensions.ReadLinesAsync(@"Resources\words-english.txt");
            int count = 0;
            await foreach (var str in lines)
            {
                count++;
            }
            Assert.IsTrue(count.Equals(26880));
        }
    }
}
