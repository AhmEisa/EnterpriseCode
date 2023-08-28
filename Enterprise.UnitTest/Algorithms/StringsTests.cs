using Enterprise.Algorithms;
using Xunit;

namespace Enterprise.UnitTest.Algorithms
{
    public class StringsTests
    {
        [Fact]
        public void DownToZero_Test()
        {
            var result = Strings.passwordCracker(new System.Collections.Generic.List<string> { "because" ,"can" ,"do", "must", "we", "what" }, "wedowhatwemustbecausewecan");

            Assert.Equal("we do what we must because we can", result);
        }
    }
}
