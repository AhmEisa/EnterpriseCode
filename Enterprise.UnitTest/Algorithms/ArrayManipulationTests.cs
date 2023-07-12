using Enterprise.Algorithms;
using System.Collections.Generic;
using Xunit;

namespace Enterprise.UnitTest.Algorithms
{
    public class ArrayManipulationTests
    {
        [Fact]
        public void ArrayManipulation_Test()
        {

            var queries = new List<List<int>> { new List<int> { 1, 2, 100 }, new List<int> { 2, 5, 100 }, new List<int> { 3, 4, 100 } };

            var result = ArrayManipulationResult.arrayManipulation(5, queries);
            Assert.Equal(200, result);
        }

        [Fact]
        public void ArrayManipulation_Test2()
        {
            SolutionFairIndex fairIndex = new SolutionFairIndex();
            int[] A = { 1, 4, 2, -2, 5 };
            int[] B = { 7, -2, -2, 2, 5 };
            
            int result = fairIndex.solution(A, B);
            Assert.Equal(2, result);
        }

        [Fact]
        public void ArrayManipulation_Test3()
        {
            SolutionFairIndex fairIndex = new SolutionFairIndex();
            int result = fairIndex.solutionB(44432);
            Assert.Equal(45010, result);
        }

    }
}
