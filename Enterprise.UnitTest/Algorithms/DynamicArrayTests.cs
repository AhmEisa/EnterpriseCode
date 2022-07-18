using Enterprise.Algorithms;
using System.Collections.Generic;
using Xunit;

namespace Enterprise.UnitTest.Algorithms
{
    public class DynamicArrayTests
    {
        [Fact]
        public void DynamicArray_Test()
        {

            var queries = new List<List<int>> {
            new List<int>{ 1,0,5},
            new List<int>{ 1,1,7},
            new List<int>{ 1,0,3},
            new List<int>{ 2,1,0},
            new List<int>{ 2,1,1},
            };

            var result = DynamicArrayResult.dynamicArray(2, queries);
            Assert.Equal(2, result.Count);
            Assert.Equal(7, result[0]);
            Assert.Equal(3, result[1]);
        }
    }
}
