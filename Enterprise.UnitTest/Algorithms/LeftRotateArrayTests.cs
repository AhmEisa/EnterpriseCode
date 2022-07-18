using Enterprise.Algorithms;
using System.Collections.Generic;
using Xunit;

namespace Enterprise.UnitTest.Algorithms
{
    public class LeftRotateArrayTests
    {
        [Fact]
        public void RotateArrayLeft_Test()
        {

            var testArray = new List<int> { 1, 2, 3, 4, 5 };

            var result = ArrayLeftRotateResult.rotateLeft(4, testArray);
            Assert.Equal(5, result[0]);
            Assert.Equal(1, result[1]);
            Assert.Equal(2, result[2]);
            Assert.Equal(3, result[3]);
            Assert.Equal(4, result[4]);
        }
    }
}
