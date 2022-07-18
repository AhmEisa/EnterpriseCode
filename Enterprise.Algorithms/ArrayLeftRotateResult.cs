using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Enterprise.Algorithms
{
    public class ArrayLeftRotateResult
    {

        /*
         * Complete the 'rotateLeft' function below.
         *
         * The function is expected to return an INTEGER_ARRAY.
         * The function accepts following parameters:
         *  1. INTEGER d
         *  2. INTEGER_ARRAY arr
         */

        public static List<int> rotateLeft(int d, List<int> arr)
        {
            d %= arr.Count;
            int numOfIterations = gcd(d, arr.Count);
            for (int i = 0; i < numOfIterations; i++) arr = Rotate(i,d, arr);

            return arr;
        }
        
        private static List<int> Rotate(int idX, int d, List<int> arr)
        {
            int tempElem = arr[idX];
            int j = idX;
            while (true)
            {
                int k = j + d;
                if (k >= arr.Count) k -= arr.Count;
                if (k == idX) break;
                arr[j] = arr[k];
                j = k;
            }

            arr[j] = tempElem;
            return arr;
        }

        private static int gcd(int a, int b)
        {
            if (b == 0)
                return a;
            else
                return gcd(b, a % b);
        }
        public static List<int> rotateLeft2(int d, List<int> arr)
        {
            List<int> originalArr = arr;
            for (int i = 1; i <= d; i++)
            {
                if (i % arr.Count == 0)
                    arr = originalArr;
                else arr = Rotate2(arr);
            }


            return arr;
        }
        private static List<int> Rotate2(List<int> arr)
        {
            var tempZeroelem = arr[0];
            for (int i = 1; i < arr.Count; i++) arr[i - 1] = arr[i];
            arr[arr.Count - 1] = tempZeroelem;
            return arr;
        }

    }
    class SolutionLeftRotate
    {
        public static void Main(string[] args)
        {
            TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

            string[] firstMultipleInput = Console.ReadLine().TrimEnd().Split(' ');

            int n = Convert.ToInt32(firstMultipleInput[0]);

            int d = Convert.ToInt32(firstMultipleInput[1]);

            List<int> arr = Console.ReadLine().TrimEnd().Split(' ').ToList().Select(arrTemp => Convert.ToInt32(arrTemp)).ToList();

            List<int> result = ArrayLeftRotateResult.rotateLeft(d, arr);

            textWriter.WriteLine(String.Join(" ", result));

            textWriter.Flush();
            textWriter.Close();
        }
    }
}
