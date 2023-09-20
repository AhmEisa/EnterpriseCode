using Enterprise.Algorithms;
using Xunit;

namespace Enterprise.UnitTest.Algorithms
{
    public class BinaryExponentiationTests
    {
        [Theory(DisplayName = "Raising base number to a bower using Recursive Method")]
        [InlineData(2, 3, 8)]
        [InlineData(3, 3, 27)]
        [InlineData(3, 2, 9)]
        [InlineData(3, 13, 1594323)]
        public void Recursive_Raising_base_To_bower_Returns_result(long basenumber, long bower, long expectedResult)
        {
            var result = BinaryExponentiation.RecBinaryBower(basenumber, bower);
            Assert.Equal(expectedResult, result);
        }

        [Theory(DisplayName = "Raising base number to a bower")]
        [InlineData(2, 3, 8)]
        [InlineData(3, 3, 27)]
        [InlineData(3, 2, 9)]
        [InlineData(3, 13, 1594323)]
        public void Raising_base_To_bower_Returns_result(long basenumber, long bower, long expectedResult)
        {
            var result = BinaryExponentiation.BinaryBower(basenumber, bower);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void BitManipulation_TurnOffRightMost1Bit()
        {
            int x = 18;
            var result = x & (x - 1);
            Assert.True(result % 2 == 0);
            Assert.Equal(16, result);
        }
        [Fact]
        public void BitManipulation_TurnONRightMost0Bit()
        {
            int x = 18;
            var result = x | (x + 1);
            Assert.True(result % 2 == 0);
            Assert.Equal(16, result);
        }
    }
}
