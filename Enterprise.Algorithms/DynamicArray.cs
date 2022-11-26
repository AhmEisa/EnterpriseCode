using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Enterprise.Algorithms
{
    public class DynamicArrayResult
    {
        /*
         * Complete the 'dynamicArray' function below.
         *
         * The function is expected to return an INTEGER_ARRAY.
         * The function accepts following parameters:
         *  1. INTEGER n
         *  2. 2D_INTEGER_ARRAY queries
         */

        public static List<int> dynamicArray(int n, List<List<int>> queries)
        {
            var result = new List<int>();
            var arr = new List<int>[n];
            int lastAnswer = 0;
            queries.ForEach(q =>
            {
                if (q[0] == 1)
                {
                    int idX = GetIdx(q[1], lastAnswer, n);
                    if (idX < 0 || idX > n) throw new IndexOutOfRangeException();
                    var idXlist = arr[idX] ?? new List<int> { };
                    idXlist.Add(q[2]);
                    arr[idX] = idXlist;
                }
                else if (q[0] == 2)
                {
                    int idX = GetIdx(q[1], lastAnswer, n);
                    if (idX < 0 || idX > n) throw new IndexOutOfRangeException();
                    var idXlist = arr[idX] ?? new List<int> { };
                    int idY = GetIdy(q[2], idXlist.Count);
                    if (idY < 0 || idY > idXlist.Count) throw new IndexOutOfRangeException();
                    lastAnswer = idXlist[idY];
                    result.Add(lastAnswer);
                }
            });

            return result;
        }

        private static int GetIdx(int x, int lastAnswer, int arrSize)
        {
            return (x ^ lastAnswer) % arrSize;
        }
        private static int GetIdy(int y, int xArrSize)
        {
            return y % xArrSize;
        }

    }

    class Solution
    {
        public static void Main(string[] args)
        {
            TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

            string[] firstMultipleInput = Console.ReadLine().TrimEnd().Split(' ');

            int n = Convert.ToInt32(firstMultipleInput[0]);

            int q = Convert.ToInt32(firstMultipleInput[1]);

            List<List<int>> queries = new List<List<int>>();

            for (int i = 0; i < q; i++)
            {
                queries.Add(Console.ReadLine().TrimEnd().Split(' ').ToList().Select(queriesTemp => Convert.ToInt32(queriesTemp)).ToList());
            }

            List<int> result = DynamicArrayResult.dynamicArray(n, queries);

            textWriter.WriteLine(String.Join("\n", result));

            textWriter.Flush();
            textWriter.Close();
        }
    }

}
