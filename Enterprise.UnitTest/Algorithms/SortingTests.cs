using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enterprise.Algorithms;
using Xunit;

namespace Enterprise.UnitTest.Algorithms
{
    public class SortingTests
    {
        [Fact]
        public void Find_N_mostFrequentNumber()
        {
            var result = SortingOperations.N_MostFrequentNumber(new[] { 3, 1, 4, 4, 5, 2, 6, 1 }, 2);
            Assert.Equal(2, result.Count);

            Assert.Equal(4, result[0].number);
            Assert.Equal(2, result[0].maxCount);

            Assert.Equal(1, result[1].number);
            Assert.Equal(2, result[1].maxCount);
        }

    }
}
