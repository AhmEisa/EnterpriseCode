using System;
using System.Collections.Generic;
using System.Linq;

namespace Enterprise.Algorithms
{
    public static class Arrays
    {
        public static void plusMinus(List<int> arr)
        {
            int posNum = arr.Count(x => x > 0);
            int negNum = arr.Count(x => x < 0);
            int zeroNum = arr.Count(x => x == 0);
            int toalCount = arr.Count;
            Console.WriteLine(((double)posNum / toalCount).ToString("F6"));
            Console.WriteLine(((double)negNum / toalCount).ToString("F6"));
            Console.WriteLine(((double)zeroNum / toalCount).ToString("F6"));
        }
        public static void miniMaxSum(List<int> arr)
        {
            var sortedArr = arr.OrderBy(x => x);
            double min = sortedArr.Take(4).Select(r => (double)r).Sum();
            double max = sortedArr.TakeLast(4).Select(r => (double)r).Sum();
            Console.WriteLine("{0} {1}", min, max);
        }
        public static int findMedian(List<int> arr)
        {
            arr.Sort();
            int indexOfMedian = arr.Count() / 2;
            return arr[indexOfMedian];
        }
        public static int lonelyinteger(List<int> a)
        {
            a.Sort();
            for (int i = 0; i < a.Count; i += 2)
            {
                if ((i == a.Count - 1) || a[i] != a[i + 1]) return a[i];
            }
            return -1;
        }
        public static int diagonalDifference(List<List<int>> arr)
        {
            int count = arr.Count;
            int jIndex = count - 1;
            int leftDigSum = 0, rightDigSum = 0;
            for (int i = 0; i < count; i++)
            {
                leftDigSum += arr[i][i];
                rightDigSum += arr[i][jIndex--];
            }
            return Math.Abs(leftDigSum - rightDigSum);
        }

        /*
               2 <= nums.length <= 104
            -109 <= nums[i] <= 109
            -109 <= target <= 109
         */
        public static int[] TwoSum(int[] nums, int target)
        {
            return new int[] { };
        }
        /*
         Input: prices = [7,1,5,3,6,4]
            Output: 5
            Explanation: Buy on day 2 (price = 1) and sell on day 5 (price = 6), profit = 6-1 = 5.
            Note that buying on day 2 and selling on day 1 is not allowed because you must buy before you sell.
        Input: prices = [7,6,4,3,1]
            Output: 0
            Explanation: In this case, no transactions are done and the max profit = 0.

         1 <= prices.length <= 105
         0 <= prices[i] <= 104
         */
        public static int MaxProfit(int[] prices)
        {
            return 0;
        }
        /*
         * Given an integer array nums, return true if any value appears at least twice in the array, and return false if every element is distinct.
         * Example 1:
         Input: nums = [1,2,3,1]
         Output: true

        Example 2:
        Input: nums = [1,2,3,4]
        Output: false
        Example 3:

        Input: nums = [1,1,1,3,3,4,3,2,4,2]
        Output: true
        1 <= nums.length <= 105
        -109 <= nums[i] <= 109
         */
        public static bool ContainsDuplicate(int[] nums)
        {
            return false;
        }
        /*
         Given an integer array nums, return an array answer such that answer[i] is equal to the product of all the elements of nums except nums[i].

        The product of any prefix or suffix of nums is guaranteed to fit in a 32-bit integer.

        You must write an algorithm that runs in O(n) time and without using the division operation.

 

        Example 1:

        Input: nums = [1,2,3,4]
        Output: [24,12,8,6]
        Example 2:

        Input: nums = [-1,1,0,-3,3]
        Output: [0,0,9,0,0]
        2 <= nums.length <= 105
-30 <= nums[i] <= 30
         */
        public static int[] ProductExceptSelf(int[] nums)
        {
            return new int[] { };
        }

        /*
         Given an integer array nums, find the subarray with the largest sum, and return its sum.
        A subarray is a contiguous non-empty sequence of elements within an array.
        Example 1:

        Input: nums = [-2,1,-3,4,-1,2,1,-5,4]
        Output: 6
        Explanation: The subarray [4,-1,2,1] has the largest sum 6.
        Example 2:

        Input: nums = [1]
        Output: 1
        Explanation: The subarray [1] has the largest sum 1.
        Example 3:

        Input: nums = [5,4,-1,7,8]
        Output: 23
        Explanation: The subarray [5,4,-1,7,8] has the largest sum 23.
        1 <= nums.length <= 105
-104 <= nums[i] <= 104
         */
        public static int MaxSubArray(int[] nums)
        {
            return 0;
        }

        /*
         Given an integer array nums, find a subarray that has the largest product, and return the product.
         The test cases are generated so that the answer will fit in a 32-bit integer.
        Example 1:

        Input: nums = [2,3,-2,4]
        Output: 6
        Explanation: [2,3] has the largest product 6.
        Example 2:

        Input: nums = [-2,0,-1]
        Output: 0
        Explanation: The result cannot be 2, because [-2,-1] is not a subarray.
 

        Constraints:

        1 <= nums.length <= 2 * 104
        -10 <= nums[i] <= 10
        The product of any prefix or suffix of nums is guaranteed to fit in a 32-bit integer
         */
        public static int MaxProduct(int[] nums)
        {
            return 0;
        }

        /*
         Suppose an array of length n sorted in ascending order is rotated between 1 and n times. 
        For example, the array nums = [0,1,2,4,5,6,7] might become:

        [4,5,6,7,0,1,2] if it was rotated 4 times.
        [0,1,2,4,5,6,7] if it was rotated 7 times.
        Notice that rotating an array [a[0], a[1], a[2], ..., a[n-1]] 1 time results in the array [a[n-1], a[0], a[1], a[2], ..., a[n-2]].

        Given the sorted rotated array nums of unique elements, return the minimum element of this array.

        You must write an algorithm that runs in O(log n) time.
        Example 1:

        Input: nums = [3,4,5,1,2]
        Output: 1
        Explanation: The original array was [1,2,3,4,5] rotated 3 times.
        Example 2:

        Input: nums = [4,5,6,7,0,1,2]
        Output: 0
        Explanation: The original array was [0,1,2,4,5,6,7] and it was rotated 4 times.
        Example 3:

        Input: nums = [11,13,15,17]
        Output: 11
        Explanation: The original array was [11,13,15,17] and it was rotated 4 times. 
 

        Constraints:

        n == nums.length
        1 <= n <= 5000
        -5000 <= nums[i] <= 5000
        All the integers of nums are unique.
        nums is sorted and rotated between 1 and n times.

        The main idea is, the element is said to be minimum in the rotated sorted array if the previous element to it is greater than it or there is no previous element(i.e. no rotation). We can do this using Binary search

        Find the mid element i.e. mid = (low+high)/2
        If the (mid+1)th element is less than mid element then return (mid+1)th element
        If the mid element is less than (mid-1)th element then return the mid element
        If the last element is greater than mid element then search in left half
        If the last element is less than mid element then search in right half

         */
        public static int FindMin(int[] nums)
        {
            return 0;
        }

        /*
         There is an integer array nums sorted in ascending order (with distinct values).

        Prior to being passed to your function, nums is possibly rotated at an unknown pivot index k (1 <= k < nums.length) such that the resulting array is [nums[k], nums[k+1], ..., nums[n-1], nums[0], nums[1], ..., nums[k-1]] (0-indexed). For example, [0,1,2,4,5,6,7] might be rotated at pivot index 3 and become [4,5,6,7,0,1,2].

        Given the array nums after the possible rotation and an integer target, return the index of target if it is in nums, or -1 if it is not in nums.

        You must write an algorithm with O(log n) runtime complexity.

 

        Example 1:

        Input: nums = [4,5,6,7,0,1,2], target = 0
        Output: 4
        Example 2:

        Input: nums = [4,5,6,7,0,1,2], target = 3
        Output: -1
        Example 3:

        Input: nums = [1], target = 0
        Output: -1
 

        Constraints:

        1 <= nums.length <= 5000
        -104 <= nums[i] <= 104
        All values of nums are unique.
        nums is an ascending array that is possibly rotated.
        -104 <= target <= 104

        First do a binary search to find the pivot:

        Step 1
        compare mid pointer to right most value on the array. If value in mid pointer is greater than right most value in array, we know the pivot is to the right of the mid pointer. If value at mid pointer is smaller than right most value in the array, we know that the pivot is to the left. Split array in half until your left and right pointers touch.
        Step 2
        Do a 2nd binary search on the whole array. This time we are looking for the target, not a pivot. Now that we know our pivot we just need to modify our midvalue like so: realMid = (mid + pivot) % nums.Length;
        Complexity of 2 binary searches is O ( 2 Log (n)) which is O (Log(n))

         */
        public static int Search(int[] nums, int target)
        {
            return 0;
        }

        /*
         Given an integer array nums, return all the triplets [nums[i], nums[j], nums[k]] such that i != j, i != k, and j != k, and nums[i] + nums[j] + nums[k] == 0.

        Notice that the solution set must not contain duplicate triplets.

 

        Example 1:

        Input: nums = [-1,0,1,2,-1,-4]
        Output: [[-1,-1,2],[-1,0,1]]
        Explanation: 
        nums[0] + nums[1] + nums[2] = (-1) + 0 + 1 = 0.
        nums[1] + nums[2] + nums[4] = 0 + 1 + (-1) = 0.
        nums[0] + nums[3] + nums[4] = (-1) + 2 + (-1) = 0.
        The distinct triplets are [-1,0,1] and [-1,-1,2].
        Notice that the order of the output and the order of the triplets does not matter.
        Example 2:

        Input: nums = [0,1,1]
        Output: []
        Explanation: The only possible triplet does not sum up to 0.
        Example 3:

        Input: nums = [0,0,0]
        Output: [[0,0,0]]
        Explanation: The only possible triplet sums up to 0.
 

        Constraints:

        3 <= nums.length <= 3000
        -105 <= nums[i] <= 105

                Sort the given array in non-decreasing order.
        Loop through the array from index 0 to n-1.
        For each iteration, set the target as -nums[i].
        Set two pointers, j=i+1 and k=n-1.
        While j<k, check if nums[j]+nums[k]==target.
        If yes, add the triplet {nums[i], nums[j], nums[k]} to the result and move j to the right and k to the left.
        If no, move either j or k based on the comparison of nums[j]+nums[k] with target.
        To avoid duplicate triplets, skip the iterations where nums[i]==nums[i-1] and also skip the iterations where nums[j]==nums[j-1] or nums[k]==nums[k+1].
         */
        public static IList<IList<int>> ThreeSum(int[] nums)
        {
            return null; 
        }

        /*
         You are given an integer array height of length n. There are n
            vertical lines drawn such that the two endpoints of the ith line are
            (i, 0) and (i, height[I]).
            Find two lines that together with the x-axis form a container, such
            that the container contains the most water.
            Return the maximum amount of water a container can store.
            Input: height = [1,8,6,2,5,4,8,3,7]
            Output: 49
            Explanation: The above vertical lines are represented by an array
            [1,8,6,2,5,4,8,3,7]. In this case, the max area of water (blue section)
            the container can contain is 49.

         */
    }
}
