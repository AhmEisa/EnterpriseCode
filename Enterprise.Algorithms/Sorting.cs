using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Algorithms
{

    public class SortingOperations
    {
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
