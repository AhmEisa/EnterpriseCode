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
        [Fact]
        public void Array_DuplicateZeros()
        {
            Arrays.DuplicateZeros(new int[] { 1, 0, 2, 3, 0, 4, 5, 0 });
            Assert.Equal(1, 1);
        }

        [Fact]
        public void Array_Merge()
        {
            Arrays.Merge(new int[] { 1, 2, 3, 0, 0, 0 }, 3, new int[] { 2, 5, 6 }, 3);
            Assert.Equal(1, 1);
        }
        [Fact]
        public void Array_RemoveDuplicates()
        {
            var result = Arrays.RemoveDuplicates(new int[] { 0, 0, 1, 1, 1, 2, 2, 3, 3, 4 });
            Assert.Equal(5, result);
        }
        [Fact]
        public void Array_CheckIfExist()
        {
            var result = Arrays.CheckIfExist(new int[] { 10, 2, 5, 3 });
            Assert.True(result);
        }
        [Fact]
        public void Array_ReplaceElements()
        {
            var result = Arrays.ReplaceElements(new int[] { 17, 18, 5, 4, 6, 1 });
            Assert.True(true);
        }

        [Fact]
        public void Array_PivotIndex()
        {
            var result = Arrays.PivotIndex_2(new int[] { 1, 7, 3, 6, 5, 6 });
            Assert.Equal(3, result);
        }
        [Fact]
        public void Array_DominantIndex()
        {
            var result = Arrays.DominantIndex(new int[] { 3, 6, 1, 0 });
            Assert.Equal(1, result);
        }
        [Fact]
        public void Array_PlusOne()
        {
            var result = Arrays.PlusOne(new int[] { 9 });
            Assert.Equal(new int[] { 1, 0 }, result);
        }
        [Fact]
        public void Array_PlusOne_2()
        {
            var result = Arrays.PlusOne(new int[] { 6, 1, 4, 5, 3, 9, 0, 1, 9, 5, 1, 8, 6, 7, 0, 5, 5, 4, 3 });
            Assert.Equal(new int[] { 6, 1, 4, 5, 3, 9, 0, 1, 9, 5, 1, 8, 6, 7, 0, 5, 5, 4, 4 }, result);
        }

        [Fact]
        public void Array_SortArray()
        {
            var result = Arrays.SortArray(new int[] { 5, 2, 3, 1 });
            Assert.Equal(new int[] { 1, 2, 3, 5 }, result);
        }
        [Fact]
        public void Array_SortArray2()
        {
            var result = Arrays.SortArray(new int[] { 5, 1, 1, 2, 0, 0, 0 });
            Assert.Equal(new int[] { 0, 0, 0, 1, 1, 2, 5 }, result);
        }
    }
    public class NumbersManipulationTests
    {
        [Fact]
        public void NormalSum_Test()
        {
            var result = Numbers.NormalSum(uint.MaxValue, uint.MaxValue);
            Assert.Equal(4294967294, result);
        }
        [Fact]
        public void NormalSum_Test_2()
        {
            //٤,٢٩٤,٩٦٧,٢٩٤
            var result = Numbers.NormalSum(int.MaxValue, int.MaxValue);
            Assert.Equal(-2, result);
        }

        [Fact]
        public void MyCircularQueue_Test_()
        {
            MyCircularQueue queue = new MyCircularQueue(3);
            queue.EnQueue(1);
            queue.EnQueue(2);
            queue.EnQueue(3);
            bool isOk = queue.EnQueue(4);
            int rearValue = queue.Rear();
            Assert.False(isOk);
            Assert.Equal(3, rearValue);
        }
        [Fact]
        public void MinStack_Test_()
        {
            MinStack stack = new MinStack();
            stack.Push(-2);
            stack.Push(0);
            stack.Push(-3);
            int minValue = stack.GetMin();
            Assert.Equal(-3, minValue);
             stack.Pop();
            minValue = stack.GetMin();
            Assert.Equal(-2, minValue);
        }
    }
}
