using Enterprise.Algorithms;
using Xunit;

namespace Enterprise.UnitTest.Algorithms
{
    public class StringsTests
    {
        [Fact]
        public void DownToZero_Test()
        {
            var result = Strings.passwordCracker(new System.Collections.Generic.List<string> { "because", "can", "do", "must", "we", "what" }, "wedowhatwemustbecausewecan");

            Assert.Equal("we do what we must because we can", result);
        }
        [Fact]
        public void Strings_timeConversion()
        {
            var result = Strings.timeConversion("07:05:45PM");

            Assert.Equal("19:05:45", result);
        }

        [Fact]
        public void Strings_LongestCommonPrefix()
        {
            var result = Strings.LongestCommonPrefix(new string[] { "flower", "flow", "flight" });

            Assert.Equal("l", result);
        }
    }
}
