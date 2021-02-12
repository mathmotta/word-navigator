using BluePrism.WordNavigator.Core.DTO;
using NUnit.Framework;
using System.Collections.Generic;

namespace BluePrism.WordNavigator.Core.Tests.DTO
{
    public class ShortestPathsDTOTests
    {
        [Test]
        public void ToString_Success()
        {
            var input = new List<ICollection<string>>();
            var inputResult1 = new List<string>();
            inputResult1.Add("spin");
            inputResult1.Add("spit");
            inputResult1.Add("spot");
            var inputResult2 = new List<string>();
            inputResult2.Add("spot");
            inputResult2.Add("spit");
            inputResult2.Add("spin");

            input.Add(inputResult1);
            input.Add(inputResult2);

            var shortestPaths = ShortestPathsDTO.CreateFrom(input).ToString();

            var expectedResult = "[ spin -> spit -> spot ],\r\n[ spot -> spit -> spin ]\r\n";

            Assert.IsTrue(shortestPaths != null);
            
        }
    }
}
