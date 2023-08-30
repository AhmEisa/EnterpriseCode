﻿using Enterprise.Algorithms;
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
        [Fact]
        public void Array_miniMaxSum()
        {
            Arrays.miniMaxSum(new List<int> { 256741038, 623958417, 467905213, 714532089, 938071625 });
            Assert.Equal(1, 1);
        }
        [Fact]
        public void Array_findMedian()
        {
            var result = Arrays.findMedian(new List<int> { 5, 3, 1, 2, 4 });
            Assert.Equal(3, result);
        }
        [Fact]
        public void Array_lonelyinteger()
        {
            var result = Arrays.lonelyinteger(new List<int> { 1, 2, 3, 4, 3, 2, 1 });
            Assert.Equal(4, result);
        }
        [Fact]
        public void Array_diagonalDifference()
        {
            var result = Arrays.diagonalDifference(new List<List<int>> { new List<int> { 11, 2, 4 }, new List<int> { 4, 5, 6 }, new List<int> { 10, 8, -12 } });
            Assert.Equal(15, result);
        }
        [Fact]
        public void Array_TwoSum()
        {
            var result = Arrays.TwoSum(new int[] { 2, 7, 11, 15 }, 9);
            Assert.Equal(new int[] { 0, 1 }, result);
        }
        [Fact]
        public void Array_TwoSum_2()
        {
            var result = Arrays.TwoSum(new int[] { 3, 2, 4 }, 6);
            Assert.Equal(new int[] { 1, 2 }, result);
        }
        [Fact]
        public void Array_TwoSum_3()
        {
            var result = Arrays.TwoSum(new int[] { 3, 3 }, 6);
            Assert.Equal(new int[] { 0, 1 }, result);
        }
    }
}
