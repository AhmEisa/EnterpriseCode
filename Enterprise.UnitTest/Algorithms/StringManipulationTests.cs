using Enterprise.Algorithms;
using Xunit;

namespace Enterprise.UnitTest.Algorithms
{
    public class StringManipulationTests
    {
        [Fact]
        public void ShiftStringCharacters_Test()
        {
            var testData = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var result = StringManipulation.ShiftStringCharacters(testData, 5, 65, 90);
            Assert.Equal("FGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDE", result);
        }

        [Fact]
        public void ShiftStringCharacters_Test2()
        {
            var testData = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var result = StringManipulation.Test(testData);
            Assert.Equal("FGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDE", result);
        }

        [Fact]
        public void ShiftStringCharacters_Test3()
        {
            var testData = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var result = StringManipulation.ShiftStringCharacters(testData, 5, 65, 90);
            Assert.Equal("FGHIJKLMNOPQRSTUVWXYZABCDE", result);
        }

        [Fact]
        public void ShiftStringCharacters_Test4()
        {
            var testData = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var result = StringManipulation.Test(testData);
            Assert.Equal("FGHIJKLMNOPQRSTUVWXYZABCDE", result);
        }

        [Fact]
        public void ShiftStringCharacters_Test5()
        {
            var testData = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower();
            var result = StringManipulation.ShiftStringCharacters(testData, 5, 'a', 'z');
            Assert.Equal("FGHIJKLMNOPQRSTUVWXYZABCDE".ToLower(), result);
        }

    }
}
