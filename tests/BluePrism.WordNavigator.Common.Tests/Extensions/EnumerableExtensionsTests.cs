using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using BluePrism.WordNavigator.Common.Extensions;


namespace BluePrism.WordNavigator.Common.Tests.Extensions
{
    public class EnumerableExtensionsTests
    {
        [Test]
        public void ToConcurrentHashSet_Success()
        {
            var enumerable = new List<int>();
            enumerable.Add(1);
            var result = enumerable.ToConcurrentHashSet();

            Assert.IsTrue(result.Contains(1));
        }
    }
}
