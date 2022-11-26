using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Algorithms
{
    public class SortNode
    {
        public int Value;
        public SortNode Next;
        public SortNode(int value)
        {
            this.Value = value;
            this.Next = null;
        }
    }

    public class SortingOperations
    {
        public static int[] InsertionSort(int[] a)
        {
            //compare the items and swap the greater one
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (a[j] > a[i])
                    {
                        var temp = a[i];
                        for (int index = i; index >= j + 1; index--)
                        {
                            a[index] = a[index - 1];
                        }
                        a[j] = temp;
                    }
                }
            }
            return a;
        }
        public static int[] InsertionSort2(int[] A)
        {
            for (var i = 1; i < A.Length; i++)
            {
                var value = A[i];
                //compare the items and swap the greater one
                int j = i - 1;
                while (j >= 0 && value < A[j])
                {
                    A[j + 1] = A[j];
                    j = j - 1;
                }
                A[j + 1] = value;
            }
            return A;
        }
        public static int[] ShellSort(int[] a)
        {
            int interval = 1;
            while (interval < a.Length / 3) interval = interval * 3 + 1;

            while (interval > 0)
            {
                for (int i = interval; i < a.Length; i++)
                {
                    int valueToInsert = a[i];
                    int j = i;
                    while (j > interval - 1 && a[j - interval] >= valueToInsert)
                    {
                        a[j] = a[j - interval];
                        j = j - interval;
                    }
                    a[j] = valueToInsert;
                }
                interval = (interval - 1) / 3;
            }
            return a;
        }

        public static int[] SelectionSort(int[] a)
        {
            //select the minimum value and swap with the current index value
            for (int i = 0; i < a.Length; i++)
            {
                int indexOfMinValue = i;
                int smallestValue = a[i];
                for (int j = i + 1; j < a.Length; j++)
                {
                    if (a[j] < smallestValue)
                    {
                        indexOfMinValue = j;
                        smallestValue = a[j];
                    }
                }
                if (indexOfMinValue != i)
                {
                    int temp = a[i];
                    a[i] = a[indexOfMinValue];
                    a[indexOfMinValue] = temp;
                }
            }
            return a;
        }
        public static int[] BubbleSort(int[] a)
        {
            //swap each pair out of order
            //can imporve moves by downwards and upwards
            bool NotSorted = true;
            while (NotSorted)
            {
                NotSorted = false;
                for (int i = 1; i < a.Length; i++)
                {
                    if (a[i] < a[i - 1])
                    {
                        int temp = a[i];
                        a[i] = a[i - 1];
                        a[i - 1] = temp;
                        NotSorted = true;
                    }
                }
            }
            return a;
        }
        public static int[] MakeHeap(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                int index = i;
                while (index != 0)
                {
                    int parentIndex = (index - 1) / 2;
                    if (arr[index] <= arr[parentIndex]) break;

                    int temp = arr[index];
                    arr[index] = arr[parentIndex];
                    arr[parentIndex] = temp;
                    index = parentIndex;
                }
            }
            return arr;
        }
        public static int RemoveTopItemFromHeap(int[] arr, int count)
        {
            int result = arr[0];
            arr[0] = arr[count - 1];
            int index = 0;
            while (true)
            {
                int child1 = index * 2 + 1;
                int child2 = index * 2 + 2;

                if (child1 >= count) child1 = index;
                if (child2 >= count) child2 = index;

                if (arr[index] >= arr[child1] && arr[index] >= arr[child2]) break;

                int swapIndex;
                if (arr[child1] > arr[child2])
                    swapIndex = child1;
                else
                    swapIndex = child2;

                int temp = arr[index];
                arr[index] = arr[swapIndex];
                arr[swapIndex] = temp;

                index = swapIndex;
            }
            return result;
        }
        public static int[] ReHeap(int[] arr, int lastIndex)
        {
            int index = 0;
            while (true)
            {
                int child1 = index * 2 + 1;
                int child2 = index * 2 + 2;

                if (child1 >= lastIndex) child1 = index;
                if (child2 >= lastIndex) child2 = index;

                if (arr[index] >= arr[child1] && arr[index] >= arr[child2]) break;

                int swapIndex;
                if (arr[child1] > arr[child2])
                    swapIndex = child1;
                else
                    swapIndex = child2;

                int temp = arr[index];
                arr[index] = arr[swapIndex];
                arr[swapIndex] = temp;

                index = swapIndex;
            }
            return arr;
        }
        public static int[] HeapSort(int[] arr)
        {
            arr = MakeHeap(arr);
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                int temp = arr[0];
                arr[0] = arr[i];
                arr[i] = temp;
                arr = ReHeap(arr, i);
            }

            return arr;
        }
        public static int[] QuickSort(int[] arr, int start, int end)
        {
            if (start >= end) return arr;

            int divider = arr[start];
            int lo = start;
            int hi = end;
            while (true)
            {
                //search from back to front
                while (arr[hi] >= divider)
                {
                    hi = hi - 1;
                    if (hi <= lo) break;
                }
                if (hi <= lo) { arr[lo] = divider; break; }
                arr[lo] = arr[hi];

                //search from front to back
                lo = lo + 1;
                while (arr[lo] < divider)
                {
                    lo = lo + 1;
                    if (lo >= hi) break;
                }
                if (lo >= hi) { lo = hi; arr[hi] = divider; break; }
                arr[hi] = arr[lo];
            }

            QuickSort(arr, start, lo - 1);
            QuickSort(arr, lo + 1, end);

            return arr;
        }
        public static int[] QuickSort2(int[] arr, int start, int end)
        {
            int pivot;
            if (end > start)
            {
                pivot = Partition(arr, start, end);
                QuickSort2(arr, start, pivot - 1);
                QuickSort2(arr, pivot + 1, end);
            }
            return arr;
        }
        private static int Partition(int[] arr, int start, int end)
        {
            int pivot_item = arr[start];
            int left = start;
            int right = end;
            while (left < right)
            {
                while (arr[left] <= pivot_item) left++;
                while (arr[right] > pivot_item) right--;
                if (left < right) Swap(arr, left, right);
            }
            arr[start] = arr[right];
            arr[right] = pivot_item;

            return right;
        }
        private static void Swap(int[] a, int start, int end)
        {
            int temp = a[start];
            a[start] = a[end];
            a[end] = temp;
        }
        public static int[] TreeSort(int[] arr)
        {
            //create binary search tree from array elements
            //traverse the created tree in inorder to get the sorted array
            return arr;
        }
        public static int[] MergeSort(int[] arr, int[] scratch, int start, int end)
        {
            if (start == end) return arr;
            int mid = (start + end) / 2;
            MergeSort(arr, scratch, start, mid);
            MergeSort(arr, scratch, mid + 1, end);

            int leftIndex = start;
            int rightIndex = mid + 1;
            int scratchIndex = leftIndex;
            while (leftIndex <= mid && rightIndex <= end)
            {
                if (arr[leftIndex] <= arr[rightIndex])
                {
                    scratch[scratchIndex] = arr[leftIndex];
                    leftIndex++;
                }
                else
                {
                    scratch[scratchIndex] = arr[rightIndex];
                    rightIndex++;
                }
                scratchIndex++;
            }
            for (int i = leftIndex; i <= mid; i++)
            {
                scratch[scratchIndex] = arr[i];
                scratchIndex++;
            }
            for (int i = rightIndex; i <= end; i++)
            {
                scratch[scratchIndex] = arr[i];
                scratchIndex++;
            }

            for (int i = 0; i < scratch.Length; i++)
                arr[i] = scratch[i];

            return arr;
        }
        public static int[] CountingSort(int[] arr, int maxValue)
        {
            //when knowing that the array contains a small range of values such as 0-1000 
            int[] counts = new int[maxValue + 1];
            for (int i = 0; i < counts.Length; i++) counts[i] = 0;
            for (int i = 0; i < arr.Length; i++) counts[arr[i]] = counts[arr[i]] + 1;
            int index = 0;
            for (int i = 0; i < counts.Length; i++)
                for (int j = 1; j <= counts[i]; j++)
                {
                    arr[index] = i;
                    index++;
                }
            return arr;
        }

        public static int[] PigeonholeSort(int[] arr, int maxValue)
        {
            SortNode[] pigeonholes = new SortNode[maxValue + 1];
            for (int i = 0; i < pigeonholes.Length; i++) pigeonholes[i] = null;

            for (int i = 0; i < arr.Length; i++)
            {
                SortNode cell = new SortNode(arr[i]);
                cell.Next = pigeonholes[arr[i]];
                pigeonholes[arr[i]] = cell;
            }

            int index = 0;
            for (int i = 0; i < pigeonholes.Length; i++)
            {
                var cell = pigeonholes[i];
                while (cell != null)
                {
                    arr[index] = cell.Value;
                    index++;
                    cell = cell.Next;
                }
            }

            return arr;
        }

        public static void insertionSort1(int n, List<int> arr)
        {
            int currentIndex = n - 1;
            int rightMostValue = arr[n - 1];
            for (int i = currentIndex - 1; i >= 0; i--)
            {
                if (arr[i] > rightMostValue)
                {
                    arr[currentIndex] = arr[i];
                    currentIndex = i;
                    Console.WriteLine(string.Join(' ', arr));
                }
            }
            arr[currentIndex] = rightMostValue;
            Console.WriteLine(string.Join(' ', arr));
        }
        public static void insertionSort2(int n, List<int> arr)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (arr[j] > arr[i])
                    {
                        var temp = arr[i];
                        for (int index = i; index >= j + 1; index--)
                        {
                            arr[index] = arr[index - 1];
                        }
                        arr[j] = temp;
                    }
                }
                if (i > 0)
                    Console.WriteLine(string.Join(' ', arr));
            }
        }
        public static int insertionSortTotalShifts(List<int> arr)
        {
            int totalShifts = 0;
            int n = arr.Count;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (arr[j] > arr[i])
                    {
                        var temp = arr[i];
                        for (int index = i; index >= j + 1; index--)
                        {
                            arr[index] = arr[index - 1];
                            totalShifts++;
                        }
                        arr[j] = temp;
                    }
                }
            }

            return totalShifts;
        }
        public static int findMedian(List<int> arr)
        {
            arr.Sort();
            int median = arr.Count % 2 == 0 ? (arr[arr.Count / 2] + arr[arr.Count / 2 - 1]) / 2 : arr[arr.Count / 2];
            return median;
        }
        public static List<int> closestNumbers(List<int> arr)
        {
            arr.Sort();
            int minDifference = int.MaxValue;
            List<int> result = new List<int>();
            for (int i = 0; i < arr.Count - 1; i++)
            {
                int difference = Math.Abs(arr[i] - arr[i + 1]);

                if (difference <= minDifference)
                {
                    if (difference < minDifference)
                    {
                        minDifference = difference;
                        result.Clear();
                    }
                    result.Add(arr[i]);
                    result.Add(arr[i + 1]);
                }
            }
            return result;
        }
        public static List<int> closestNumbers2(List<int> arr)
        {
            arr.Sort();
            int minDifference = int.MaxValue;
            for (int i = 0; i < arr.Count - 1; i++)
            {
                minDifference = Math.Min(minDifference, Math.Abs(arr[i + 1] - arr[i]));
            }

            List<int> result = new List<int>();
            for (int i = 0; i < arr.Count - 1; i++)
            {
                if (Math.Abs(arr[i + 1] - arr[i]) == minDifference)
                {
                    result.Add(arr[i]);
                    result.Add(arr[i + 1]);
                }
            }
            return result;
        }

        //Most frequent element in an array
        /*
         Input : arr[] = {1, 3, 2, 1, 4, 1}
         Output : 1 as 1 appears three times in array which is maximum frequency.

         Input : arr[] = {10, 20, 10, 20, 30, 20, 20}
         Output : 20

        Time Complexity: O(n Log n) 
        Auxiliary Space: O(1)
         */
        public static (int number, int maxCount) MostFrequentUsingSorting(int[] arr)
        {
            // Sort the array
            Array.Sort(arr);

            // find the max frequency using
            // linear traversal
            int max_count = 1, res = arr[0];
            int curr_count = 1;

            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] == arr[i - 1])
                    curr_count++;
                else
                {
                    if (curr_count > max_count)
                    {
                        max_count = curr_count;
                        res = arr[i - 1];
                    }
                    curr_count = 1;
                }
            }

            // If last element is most frequent
            if (curr_count > max_count)
            {
                max_count = curr_count;
                res = arr[arr.Length - 1];
            }

            return (res, max_count);
        }

        /*
         Time Complexity : O(n) 
         Auxiliary Space : O(n)
         */
        public static (int number, int maxCount) MostFrequentUsingHashing(int[] arr)
        {
            // Insert all elements in hash
            Dictionary<int, int> hashedElments = new Dictionary<int, int>();

            for (int i = 0; i < arr.Length; i++)
            {
                int key = arr[i];
                if (hashedElments.ContainsKey(key))
                {
                    int freq = hashedElments[key];
                    freq++;
                    hashedElments[key] = freq;
                }
                else
                    hashedElments.Add(key, 1);
            }

            // find max frequency.
            int min_count = 0, res = -1;

            foreach (KeyValuePair<int, int> keyValuePair in hashedElments)
            {
                if (min_count < keyValuePair.Value)
                {
                    res = keyValuePair.Key;
                    min_count = keyValuePair.Value;
                }
            }
            return (res, min_count);
        }

        //Find k numbers with most occurrences in the given array
        /*
            Input: arr[] = {3, 1, 4, 4, 5, 2, 6, 1}, k = 2
            Output: 4 1
            Explanation:
            Frequency of 4 = 2
            Frequency of 1 = 2
            These two have the maximum frequency and
            4 is larger than 1.

         */

        /*
         * Time Complexity: O(d log d), where d is the count of distinct elements in the array. To sort the array O(d log d) time is needed.
         * Auxiliary Space: O(d), where d is the count of distinct elements in the array. To store the elements in HashMap O(d) space complexity is needed.
         */
        public static List<(int number, int maxCount)> N_MostFrequentNumber(int[] arr, int k)
        {
            // Insert all elements in hash
            Dictionary<int, int> hashedElments = new Dictionary<int, int>();

            for (int i = 0; i < arr.Length; i++)
            {
                int key = arr[i];
                if (hashedElments.ContainsKey(key))
                {
                    int freq = hashedElments[key];
                    freq++;
                    hashedElments[key] = freq;
                }
                else
                    hashedElments.Add(key, 1);
            }

            return hashedElments.OrderByDescending(x => x.Value).ThenByDescending(x => x.Key).Take(k).Select(r => (r.Key, r.Value)).ToList();
        }



        //Find the smallest and second smallest elements in an array
        /* Function to print first smallest and second smallest elements 
         A Simple Solution is to sort the array in increasing order. The first two elements in sorted array would be two smallest elements. Time complexity of this solution is O(n Log n).
         A Better Solution is to scan the array twice. In first traversal find the minimum element. Let this element be x. In second traversal, find the smallest element greater than x. Time complexity of this solution is O(n).
         */
        public static (int FirstNumber, int SecondNumber) Find2Smallest(int[] arr)
        {
            int first, second, arr_size = arr.Length;

            if (arr_size < 2)
            {
                return (int.MaxValue, int.MaxValue);
            }

            first = second = int.MaxValue;

            for (int i = 0; i < arr_size; i++)
            {
                if (arr[i] < first)
                {
                    second = first;
                    first = arr[i];
                }

                else if (arr[i] < second && arr[i] != first)
                    second = arr[i];
            }

            return (first, second);
        }

    }
}
