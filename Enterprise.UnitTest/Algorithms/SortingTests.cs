using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enterprise.Algorithms;
using Pose;
using Xunit;

namespace Enterprise.UnitTest.Algorithms
{
    public class SortingTests
    {
        [Fact]
        public void TestMethodWithDate()
        {
            Shim shim = Shim.Replace(() => DateTime.Now).With(() => new DateTime(2021, 07, 20));
            PoseContext.Isolate(() =>
            {
                var testData = new List<int> { -20, -3916237, -357920, -3620601, 7374819, -7330761, 30, 6246457, -6461594, 266854, -520, -470 };
                var result = SortingOperations.closestNumbers(testData);
                Assert.NotNull(result);
            }, shim);
        }

        [Fact]
        public void ClosestNumber_Test()
        {
            var testData = new List<int> { -20, -3916237, -357920, -3620601, 7374819, -7330761, 30, 6246457, -6461594, 266854, -520, -470 };
            var result = SortingOperations.closestNumbers(testData);
            Assert.NotNull(result);
            Assert.Equal(-520, result[0]);
        }

        [Fact]
        public void InsertionSort_Test()
        {
            var testData = new int[] { 7, 5, 6, 1, 3, 2, 8, 0 };
            var result = SortingOperations.InsertionSort(testData);
            Assert.NotNull(result);
            Assert.Equal(0, result[0]);
        }

        [Fact]
        public void ShellSort_Test()
        {
            var testData = new int[] { 7, 5, 6, 1, 3, 2, 8, 0, 4 };
            var result = SortingOperations.ShellSort(testData);
            Assert.NotNull(result);
            Assert.Equal(0, result[0]);
        }

        [Fact]
        public void InsertionSort_Test1()
        {
            var testData = new int[] { 2, 4, 6, 8, 3 }.ToList();
            SortingOperations.insertionSort1(testData.Count, testData);
            Assert.NotNull(testData);
            Assert.Equal(3, testData[1]);
        }

        [Fact]
        public void SelectionSort_Test()
        {
            var testData = new int[] { 7, 5, 6, 1, 3, 2, 8, 0 };
            var result = SortingOperations.SelectionSort(testData);
            Assert.NotNull(result);
            Assert.Equal(0, result[0]);
        }

        [Fact]
        public void BubbleSort_Test()
        {
            var testData = new int[] { 7, 5, 6, 1, 3, 2, 8, 0 };
            var result = SortingOperations.BubbleSort(testData);
            Assert.NotNull(result);
            Assert.Equal(0, result[0]);
        }

        [Fact]
        public void HeapSort_Test()
        {
            var testData = new int[] { 7, 1, 10, 4, 6, 9, 2, 11, 3, 5, 12, 8 };
            var result = SortingOperations.HeapSort(testData);
            Assert.NotNull(result);
            Assert.Equal(1, result[0]);
        }

        [Fact]
        public void HeapSort_Test2()
        {
            var testData = new int[] { 7, 5, 6, 1, 3, 2, 8, 0 };
            var result = SortingOperations.HeapSort(testData);
            Assert.NotNull(result);
            Assert.Equal(0, result[0]);
        }

        [Fact]
        public void QuickSort_Test()
        {
            var testData = new int[] { 7, 1, 10, 4, 6, 9, 2, 11, 3, 5, 12, 8 };
            var result = SortingOperations.QuickSort(testData, 0, testData.Length - 1);
            Assert.NotNull(result);
            Assert.Equal(1, result[0]);
        }

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
