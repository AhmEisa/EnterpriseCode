using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Enterprise.Algorithms
{
    public class ArrayManipulationResult
    {

        /*
         * Complete the 'arrayManipulation' function below.
         *
         * The function is expected to return a LONG_INTEGER.
         * The function accepts following parameters:
         *  1. INTEGER n
         *  2. 2D_INTEGER_ARRAY queries
         */

        public static long arrayManipulation(int n, List<List<int>> queries)
        {
            var arr = new long[n]; //Enumerable.Repeat(0, n).ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = 0;
            }

            queries.ForEach(item =>
            {
                int start = item[0] - 1;
                int end = item[1];
                arr[start] += item[2];
                if (end <= (arr.Length - 1))
                    arr[end] -= item[2];
            });

            long maxValue = arr[0];

            for (int i = 1; i < arr.Length; i++)
            {
                arr[i] += arr[i - 1];
                maxValue = Math.Max(maxValue, arr[i]);
            }
            return maxValue;
        }
        public static long arrayManipulation2(int n, List<List<int>> queries)
        {
            int maxValue = 0;
            var arr = Enumerable.Repeat(0, n).ToArray();

            queries.ForEach(item =>
            {
                int start = item[0] - 1;
                int end = item[1] - 1;
                for (int i = start; i <= end; i++)
                {
                    arr[i] += item[2];
                    maxValue = Math.Max(maxValue, arr[i]);
                }
            });

            return maxValue;
        }
    }

    class ArrayManipulationSolution
    {
        public static void Main(string[] args)
        {
            TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

            string[] firstMultipleInput = Console.ReadLine().TrimEnd().Split(' ');

            int n = Convert.ToInt32(firstMultipleInput[0]);

            int m = Convert.ToInt32(firstMultipleInput[1]);

            List<List<int>> queries = new List<List<int>>();

            for (int i = 0; i < m; i++)
            {
                queries.Add(Console.ReadLine().TrimEnd().Split(' ').ToList().Select(queriesTemp => Convert.ToInt32(queriesTemp)).ToList());
            }

            long result = ArrayManipulationResult.arrayManipulation(n, queries);

            textWriter.WriteLine(result);

            textWriter.Flush();
            textWriter.Close();
        }
    }

}
