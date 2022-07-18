using Enterprise.Algorithms;
using System.Collections.Generic;
using Xunit;

namespace Enterprise.UnitTest.Algorithms
{
    public class MatchingStringsTests
    {
        [Fact]
        public void MatchStrings_Test()
        {

            var strings = new List<string> { "aba", "baba", "aba", "xzxb" };
            var queries = new List<string> { "aba", "xzxb", "ab" };

            var result = MatchingStringsResult.matchingStrings(strings, queries);
            Assert.Equal(2, result[0]);
            Assert.Equal(1, result[1]);
            Assert.Equal(0, result[2]);
        }
    }
}
