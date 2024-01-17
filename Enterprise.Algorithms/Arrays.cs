using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            Dictionary<int, int> map = new Dictionary<int, int>();

            for (int i = 0; i < nums.Length; i++)
            {
                int complement = target - nums[i];
                if (map.ContainsKey(complement))
                {
                    return new int[] { map[complement], i };
                }

                map[nums[i]] = i;
            }
            return new int[0];
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
            int lsf = int.MaxValue;
            int op = 0;
            int pist;

            for (int i = 0; i < prices.Length; i++)
            {
                if (prices[i] < lsf)
                {
                    lsf = prices[i];
                }
                pist = prices[i] - lsf;
                if (op < pist)
                {
                    op = pist;
                }
            }
            return op;
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
            Dictionary<int, int> seen = new Dictionary<int, int>();
            foreach (int num in nums)
            {
                if (seen.ContainsKey(num) && seen[num] >= 1)
                    return true;
                seen.Add(num, 1);
            }
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
            int n = nums.Length;
            int[] ans = new int[n];
            int pro = 1;
            foreach (int i in nums)
            {
                pro *= i;
            }

            for (int i = 0; i < n; i++)
            {
                ans[i] = pro / nums[i];
            }
            return ans;
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
            int maxEnding = nums[0];
            int max = nums[0];
            for (int i = 1; nums.Length > i; i++)
            {
                maxEnding = Math.Max(nums[i], maxEnding + nums[i]);
                max = Math.Max(max, maxEnding);
            }
            return max;
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
            int ans = nums[0];
            int n = nums.Count();
            int p = 1, q = 1;
            for (int i = 0; i < n; i++)
            {
                // reset to 1 when the product becomes zero
                p = (p == 0 ? 1 : p) * nums[i];
                q = (q == 0 ? 1 : q) * nums[n - 1 - i];
                ans = Math.Max(ans, Math.Max(p, q));
            }
            return ans;
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
            List<IList<int>> output = new List<IList<int>>();
            Array.Sort(nums);

            if ((nums[0] >= 0 && nums[nums.Length - 1] != 0) || nums[nums.Length - 1] < 0)
            {
                return new List<IList<int>>();
            }


            for (int index = 0; index < nums.Length; index++)
            {
                int num = nums[index];

                if (index > 0 && num == nums[index - 1]) continue;

                if (num > 0) break;

                int left = index + 1;
                int right = nums.Length - 1;

                while (left < right)
                {
                    int threeSum = num + nums[left] + nums[right];

                    if (threeSum > 0) right--;
                    else if (threeSum < 0) left++;
                    else
                    {
                        IList<int> new_array = new List<int>() { nums[index], nums[left], nums[right] };
                        output.Add(new_array);
                        left++;
                        while (nums[left] == nums[left - 1] && left < right)
                        {
                            left++;
                        }
                    }
                }
            }
            return output;
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

        /*
         Example 1:

        Input: nums = [1,1,0,1,1,1]
        Output: 3
        Explanation: The first two digits or the last three digits are consecutive 1s. The maximum number of consecutive 1s  is 3
         */
        public static int FindMaxConsecutiveOnes(int[] nums)
        {
            int maxConsecutiveOnes = 0;
            int currentOnesCount = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] == 1) currentOnesCount += 1;
                else { maxConsecutiveOnes = Math.Max(currentOnesCount, maxConsecutiveOnes); currentOnesCount = 0; }
            }
            return Math.Max(currentOnesCount, maxConsecutiveOnes);
        }
        public static int FindMaxConsecutiveOnes_2(int[] nums)
        {
            int n = nums.Length;
            int max = 0, left = 0, right = 0;
            while (left < n && right < n)
            {
                while (left < n && nums[left] == 0)
                    left++;
                right = left;
                while (right < n && nums[right] == 1)
                    right++;
                max = Math.Max(max, right - left);
                left = right;
            }
            return max;
        }
        public static int FindMaxConsecutiveOnes_3(int[] nums)
        {
            int m = nums[0];
            for (int i = 1; i < nums.Length; i++)
            {
                nums[i] = nums[i] * (nums[i] + nums[i - 1]);
                m = Math.Max(nums[i], m);
            }
            return m;
        }

        /*
         Given an array nums of integers, return how many of them contain an even number of digits.
        Input: nums = [12,345,2,6,7896]
        Output: 2
        Explanation: 
        12 contains 2 digits (even number of digits). 
        345 contains 3 digits (odd number of digits). 
        2 contains 1 digit (odd number of digits). 
        6 contains 1 digit (odd number of digits). 
        7896 contains 4 digits (even number of digits). 
        Therefore only 12 and 7896 contain an even number of digits.
         */
        public static int FindNumbers(int[] nums)
        {
            int numberWithEvenDigitsCount = 0;
            for (int i = 0; i < nums.Length; i++)
                if (IsEvenNumberOfDigitsInNumber(nums[i]))
                    numberWithEvenDigitsCount++;
            return numberWithEvenDigitsCount;
        }
        private static bool IsEvenNumberOfDigitsInNumber(int num)
        {
            int result = 0;
            while (num > 0)
            {
                num /= 10;
                result++;
            }
            return result % 2 == 0;
        }
        /*
         Given an integer array nums sorted in non-decreasing order, return an array of the squares of each number sorted in non-decreasing order.

 

        Example 1:

        Input: nums = [-4,-1,0,3,10]
        Output: [0,1,9,16,100]
        Explanation: After squaring, the array becomes [16,1,0,9,100].
        After sorting, it becomes [0,1,9,16,100].
        Example 2:

        Input: nums = [-7,-3,2,3,11]
        Output: [4,9,9,49,121]
         */
        public static int[] SortedSquares(int[] nums)
        {
            int[] result = new int[nums.Length];
            for (int i = 0; i < nums.Length; i++)
            {
                result[i] = nums[i] * nums[i];
            }
            Array.Sort(result);
            return result;
        }
        public static int[] SortedSquares_2(int[] nums)
        {
            int n = nums.Length;
            int[] result = new int[n];
            int left = 0;
            int right = n - 1;

            for (int i = n - 1; i >= 0; i--)
            {
                int square;
                if (Math.Abs(nums[left]) < Math.Abs(nums[right]))
                {
                    square = nums[right];
                    right--;
                }
                else
                {
                    square = nums[left];
                    left++;
                }
                result[i] = square * square;
            }
            return result;
        }
        /*
         Given a fixed-length integer array arr, duplicate each occurrence of zero, shifting the remaining elements to the right.

        Note that elements beyond the length of the original array are not written. Do the above modifications to the input array in place and do not return anything.

 

        Example 1:

        Input: arr = [1,0,2,3,0,4,5,0]
        Output: [1,0,0,2,3,0,0,4]
        Explanation: After calling your function, the input array is modified to: [1,0,0,2,3,0,0,4]
        Example 2:

        Input: arr = [1,2,3]
        Output: [1,2,3]
        Explanation: After calling your function, the input array is modified to: [1,2,3]
         */
        public static void DuplicateZeros(int[] arr)
        {
            if (arr == null || arr.Length == 0) return;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == 0)
                {
                    ShiftArrayToRightOfIndex(arr, i);
                    i++; // we don't want to traverse over the duplicate zero
                }
            }
        }

        private static void ShiftArrayToRightOfIndex(int[] arr, int index)
        {
            for (int j = arr.Length - 1; j > index; j--)
            {
                arr[j] = arr[j - 1];
            }
        }
        public static void duplicateZeros(int[] arr)
        {
            int possibleDups = 0;
            int length_ = arr.Length - 1;

            // Find the number of zeros to be duplicated
            // Stopping when left points beyond the last element in the original array
            // which would be part of the modified array
            for (int left = 0; left <= length_ - possibleDups; left++)
            {
                // Count the zeros
                if (arr[left] == 0)
                {
                    // Edge case: This zero can't be duplicated. We have no more space,
                    // as left is pointing to the last element which could be included  
                    if (left == length_ - possibleDups)
                    {
                        // For this zero we just copy it without duplication.
                        arr[length_] = 0;
                        length_ -= 1;
                        break;
                    }
                    possibleDups++;
                }
            }

            // Start backwards from the last element which would be part of new array.
            int last = length_ - possibleDups;

            // Copy zero twice, and non zero once.
            for (int i = last; i >= 0; i--)
            {
                if (arr[i] == 0)
                {
                    arr[i + possibleDups] = 0;
                    possibleDups--;
                    arr[i + possibleDups] = 0;
                }
                else
                {
                    arr[i + possibleDups] = arr[i];
                }
            }
        }
        /*
         You are given two integer arrays nums1 and nums2, sorted in non-decreasing order, and two integers m and n, representing the number of elements in nums1 and nums2 respectively.

            Merge nums1 and nums2 into a single array sorted in non-decreasing order.

            The final sorted array should not be returned by the function, but instead be stored inside the array nums1. To accommodate this, nums1 has a length of m + n, where the first m elements denote the elements that should be merged, and the last n elements are set to 0 and should be ignored. nums2 has a length of n.

 

            Example 1:

            Input: nums1 = [1,2,3,0,0,0], m = 3, nums2 = [2,5,6], n = 3
            Output: [1,2,2,3,5,6]
            Explanation: The arrays we are merging are [1,2,3] and [2,5,6].
            The result of the merge is [1,2,2,3,5,6] with the underlined elements coming from nums1.
            Example 2:

            Input: nums1 = [1], m = 1, nums2 = [], n = 0
            Output: [1]
            Explanation: The arrays we are merging are [1] and [].
            The result of the merge is [1].
            Example 3:

            Input: nums1 = [0], m = 0, nums2 = [1], n = 1
            Output: [1]
            Explanation: The arrays we are merging are [] and [1].
            The result of the merge is [1].
            Note that because m = 0, there are no elements in nums1. The 0 is only there to ensure the merge result can fit in  nums1.
                     */
        public static void merge(int[] nums1, int m, int[] nums2, int n)
        {
            for (int i = 0; i < n; i++)
            {
                nums1[i + m] = nums2[i];
            }
            Array.Sort(nums1);
        }
        public static void merge_2(int[] nums1, int m, int[] nums2, int n)
        {
            // Make a copy of the first m elements of nums1.
            int[] nums1Copy = new int[m];
            for (int i = 0; i < m; i++)
            {
                nums1Copy[i] = nums1[i];
            }

            // Read pointers for nums1Copy and nums2 respectively.
            int p1 = 0;
            int p2 = 0;

            // Compare elements from nums1Copy and nums2 and write the smallest to nums1.
            for (int p = 0; p < m + n; p++)
            {
                // We also need to ensure that p1 and p2 aren't over the boundaries
                // of their respective arrays.
                if (p2 >= n || (p1 < m && nums1Copy[p1] < nums2[p2]))
                {
                    nums1[p] = nums1Copy[p1++];
                }
                else
                {
                    nums1[p] = nums2[p2++];
                }
            }
        }
        public static void Merge(int[] nums1, int m, int[] nums2, int n)
        {
            int p1 = m - 1;
            int p2 = n - 1;

            // And move p backwards through the array, each time writing
            // the smallest value pointed at by p1 or p2.
            for (int p = m + n - 1; p >= 0; p--)
            {
                if (p2 < 0)
                {
                    break;
                }
                if (p1 >= 0 && nums1[p1] > nums2[p2])
                {
                    nums1[p] = nums1[p1--];
                }
                else
                {
                    nums1[p] = nums2[p2--];
                }
            }
        }

        /*
         Given an integer array nums and an integer val, remove all occurrences of val in nums in-place. The order of the elements may be changed. Then return the number of elements in nums which are not equal to val.

        Consider the number of elements in nums which are not equal to val be k, to get accepted, you need to do the following things:

        Change the array nums such that the first k elements of nums contain the elements which are not equal to val. The remaining elements of nums are not important as well as the size of nums.
        Return k.
         */
        public static int RemoveElement(int[] nums, int val)
        {
            int last = nums.Length;
            for (int p = 0; p < last;)
            {
                if (nums[p] == val)
                {
                    nums[p] = nums[last - 1];
                    last--;
                }
                else { p++; }

            }
            return last;
        }
        public static int removeElement(int[] nums, int val)
        {
            int i = 0;
            for (int j = 0; j < nums.Length; j++)
            {
                if (nums[j] != val)
                {
                    nums[i] = nums[j];
                    i++;
                }
            }
            return i;
        }
        /*
         Given an integer array nums sorted in non-decreasing order, remove the duplicates in-place such that each unique element appears only once. The relative order of the elements should be kept the same. Then return the number of unique elements in nums.

            Consider the number of unique elements of nums to be k, to get accepted, you need to do the following things:

            Change the array nums such that the first k elements of nums contain the unique elements in the order they were present in nums initially. The remaining elements of nums are not important as well as the size of nums.
            Return k.
         */
        public static int RemoveDuplicates(int[] nums)
        {
            int left = 0, right = 1;
            while (right < nums.Length)
            {
                if (nums[right] == nums[left]) { right++; }
                else { nums[left + 1] = nums[right++]; left++; }
            }
            return left + 1;
        }

        /*
         Given an array arr of integers, check if there exist two indices i and j such that :

            i != j
            0 <= i, j < arr.length
            arr[i] == 2 * arr[j]
 

            Example 1:

            Input: arr = [10,2,5,3]
            Output: true
            Explanation: For i = 0 and j = 2, arr[i] == 10 == 2 * 5 == 2 * arr[j]
            Example 2:

            Input: arr = [3,1,7,11]
            Output: false
            Explanation: There is no i and j that satisfy the conditions.
         */
        public static bool CheckIfExist(int[] arr)
        {
            Dictionary<int, int> pairs = new Dictionary<int, int>();
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] % 2 == 0 && pairs.ContainsKey(arr[i] / 2) || pairs.ContainsKey(arr[i] * 2)) return true;
                if (!pairs.ContainsKey(arr[i])) { pairs.Add(arr[i], i); }
            }
            return false;
        }
        /*
         Given an array of integers arr, return true if and only if it is a valid mountain array.

        Recall that arr is a mountain array if and only if:

        arr.length >= 3
        There exists some i with 0 < i < arr.length - 1 such that:
        arr[0] < arr[1] < ... < arr[i - 1] < arr[i]
        arr[i] > arr[i + 1] > ... > arr[arr.length - 1]
         */
        public static bool ValidMountainArray(int[] arr)
        {
            int i = 0;
            while (i + 1 < arr.Length && arr[i] < arr[i + 1])
                i++;
            if (i == 0 || i == arr.Length - 1) return false;

            while (i + 1 < arr.Length && arr[i] > arr[i + 1])
                i++;

            return i == arr.Length - 1;
        }

        /*
         Given an array arr, replace every element in that array with the greatest element among the elements to its right, and replace the last element with -1.

            After doing so, return the array.
         */
        public static int[] ReplaceElements(int[] arr)
        {
            Dictionary<int, int> pairs = new Dictionary<int, int>();
            pairs.Add(arr.Length - 1, arr[arr.Length - 1]);
            for (int i = arr.Length - 2; i > 0; i--)
            {
                pairs.Add(i, Math.Max(pairs[i + 1], arr[i]));
            }
            for (int i = 0; i < arr.Length - 1; i++)
                arr[i] = pairs[i + 1];
            arr[arr.Length - 1] = -1;
            return arr;
        }
        /*
         Given an array of integers nums, calculate the pivot index of this array.

        The pivot index is the index where the sum of all the numbers strictly to the left of the index is equal to the sum of all the numbers strictly to the index's right.

        If the index is on the left edge of the array, then the left sum is 0 because there are no elements to the left. This also applies to the right edge of the array.

        Return the leftmost pivot index. If no such index exists, return -1.
         */
        public static int PivotIndex(int[] nums)
        {
            int[] aggregatedLeftSums = new int[nums.Length];
            int[] aggregatedRightSums = new int[nums.Length];
            int lastIndex = nums.Length - 1;
            aggregatedLeftSums[0] = nums[0];
            aggregatedRightSums[lastIndex] = nums[lastIndex];
            for (int i = 1; i < nums.Length; i++)
            {
                aggregatedLeftSums[i] = aggregatedLeftSums[i - 1] + nums[i];
                aggregatedRightSums[lastIndex - i] = aggregatedRightSums[lastIndex - i + 1] + nums[lastIndex - i];
            }
            for (int i = 0; i < nums.Length; i++)
            { if (aggregatedLeftSums[i] == aggregatedRightSums[i]) return i; }
            return -1;
        }
        public static int PivotIndex_2(int[] nums)
        {
            int leftSum = 0, totalSum = 0;
            foreach (int item in nums) totalSum += item;
            for (int i = 0; i < nums.Length; i++)
            { if (leftSum == totalSum - leftSum - nums[i]) return i; leftSum += nums[i]; }
            return -1;
        }

        /*
         You are given an integer array nums where the largest integer is unique.
         Determine whether the largest element in the array is at least twice as much as every other number in the array. If it is, return the index of the largest element, or return -1 otherwise.
         */
        public static int DominantIndex(int[] nums)
        {
            int largestElementIndex = 0;
            int largestValue = nums[0];
            for (int i = 1; i < nums.Length; i++) { if (nums[i] > largestValue) { largestValue = nums[i]; largestElementIndex = i; } }
            for (int i = 0; i < nums.Length; i++)
            {
                if (i == largestElementIndex) continue;
                if (nums[i] * 2 > largestValue) return -1;
            }
            return largestElementIndex;
        }

        /*
         You are given a large integer represented as an integer array digits, where each digits[i] is the ith digit of the integer. The digits are ordered from most significant to least significant in left-to-right order. The large integer does not contain any leading 0's.

         Increment the large integer by one and return the resulting array of digits.
         */
        public static int[] PlusOne(int[] digits)
        {
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                if (digits[i] == 9) digits[i] = 0;
                else { digits[i]++; return digits; }
            }
            int[] result = new int[digits.Length + 1];
            result[0] = 1;
            return result;
        }
        private static double GetLargeInteger(int[] digits)
        {
            double result = 0;
            int powerIndex = 0;
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                result += digits[i] * Math.Pow(10, powerIndex);
                powerIndex++;
            }
            return result;
        }

        public static int[] SortArray(int[] nums)
        {
            if (nums.Length <= 1) return nums;
            int pivot = nums.Length / 2;
            int[] leftArray = new int[pivot], rightArray = new int[nums.Length - pivot];
            Array.Copy(nums, 0, leftArray, 0, pivot);
            Array.Copy(nums, pivot, rightArray, 0, nums.Length - pivot);
            int[] leftResult = SortArray(leftArray); //// divide & conqer
            int[] rightResult = SortArray(rightArray);//// divide & conqer
            return Merge(leftResult, rightResult); // Combine
        }

        private static int[] Merge(int[] leftResult, int[] rightResult)
        {
            int left_index = 0, right_index = 0, curr_index = 0;
            int[] ret = new int[leftResult.Length + rightResult.Length];
            while (left_index < leftResult.Length && right_index < rightResult.Length)
            {
                if (leftResult[left_index] < rightResult[right_index]) { ret[curr_index++] = leftResult[left_index++]; }
                else { ret[curr_index++] = rightResult[right_index++]; }
            }
            while (left_index < leftResult.Length)
            {
                ret[curr_index++] = leftResult[left_index++];
            }
            while (right_index < rightResult.Length)
            {
                ret[curr_index++] = rightResult[right_index++];
            }
            return ret;
        }
    }
    public class MyCircularQueue
    {

        private int[] _queue;
        private int _front = -1, _rear = -1;
        public MyCircularQueue(int k)
        {
            _queue = new int[k];
        }

        public bool EnQueue(int value)
        {
            if (IsFull()) return false;
            _rear = (_rear + 1) % _queue.Length;
            _queue[_rear] = value;
            return true;
        }

        public bool DeQueue()
        {
            if (IsEmpty()) return false;
            _front = _front == -1 ? 0 : _front;
            _front = (_front + 1) % _queue.Length;
            return true;
        }

        public int Front()
        {
            return _queue[_front];
        }

        public int Rear()
        {
            return _queue[_rear];
        }

        public bool IsEmpty()
        {
            return _front == _rear;
        }

        public bool IsFull()
        {
            return _front == (_rear + 1) % _queue.Length;
        }
    }
    public class MovingAverage
    {
        private Queue<int> _window;
        private int _size = 0;
        public MovingAverage(int size)
        {
            _window = new Queue<int>();
            _size = size;
        }

        public double Next(int val)
        {
            if (_window.Count != 0 && _window.Count >= _size) _window.Dequeue();
            _window.Enqueue(val);
            int sum = 0;
            foreach (var item in _window) { sum += item; }
            return sum / (double)_window.Count;
        }
    }
    public class MinStack
    {
        private Stack<(int, int)> _stack;
        public MinStack()
        {
            _stack = new Stack<(int, int)>();
        }

        public void Push(int val)
        {
            int top = _stack.Count == 0 ? val : this.Top().Item1;
            _stack.Push((val, Math.Min(val, top)));
        }

        public void Pop()
        {
            if (_stack.Count == 0) return;
            _stack.Pop();
        }

        public (int, int) Top()
        {
            if (_stack.Count == 0) return (-1, -1);
            return _stack.Peek();
        }

        public int GetMin()
        {
            if (_stack.Count == 0) return -1;
            return this.Top().Item2;
        }
    }

}
