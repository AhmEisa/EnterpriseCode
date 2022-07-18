using Encryption.CaesarCipher;
using Enterprise.Try;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        private static int _counter = 0;
        private static void DoCount()
        {
            int _internalCount = 0;
            for (int i = 0; i < 100000; i++)
            {
                _internalCount++;
            }
            _counter += _internalCount;
        }
        static void Main(string[] args)
        {
            //         int threadsCount = 100;
            //Thread[] threads = new Thread[threadsCount];

            ////Allocate threads
            //for (int i = 0; i < threadsCount; i++)
            //	threads[i] = new Thread(DoCount);

            ////Start threads
            //for (int i = 0; i < threadsCount; i++)
            //	threads[i].Start();

            ////Wait for threads to finish
            //for (int i = 0; i < threadsCount; i++)
            //	threads[i].Join();

            // var result =  CalcPoints(new[] { "5", "2", "C","D","+" });
            //var result = CountElems(new[] { 1, 2, 3 });
            // var result = findNumbers(new[] { 1,2,3,4,3});
            var root = new TreeNode { Value = 3, Left = new TreeNode { Value = 9 }, Right = new TreeNode { Value = 20, Left = new TreeNode { Value = 15 }, Right = new TreeNode { Value = 7 } } };
            //var result = SumOfLeafes(root);
            //Print counter
            // Console.WriteLine("Counter is: {0}", result);

            // var textExtracted = ExtractTextFromFiles.ExtractText(@"D:\Work Files\Techno-Ways\Geidea Gateway_ DirectAPI Guide v1.2.pdf");
            // Console.WriteLine("Text in pdf : {0}", textExtracted);

            //DocumentStore documentStore = new DocumentStore(2);
            //documentStore.AddDocument("item");
            //Console.WriteLine(documentStore);

            //ChainLink left = new ChainLink();
            //ChainLink middle = new ChainLink();
            //ChainLink right = new ChainLink();
            //left.Append(middle);
            //middle.Append(right);
            //Console.WriteLine(left.LongerSide());

            //CodePerformance.Test();

            // PlusMinsNumberDigitsToBeZero.MainMethod();
            // SubtractDigitsFromString.Subtract("This is string containing 22 and 43");

            //int result = int.Parse("25.0");

            // var totalDays =  (new DateTime(2022, 5, 12) - new DateTime(2022, 5, 12)).TotalDays+1;
            // var value = (float)28861633;

            TestContracts();

            Console.ReadLine();
        }

        public static void TestContracts()
        {
            //TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

            int queriesRows = Convert.ToInt32(Console.ReadLine().Trim());

            List<List<string>> queries = new List<List<string>>();

            for (int i = 0; i < queriesRows; i++)
            {
                queries.Add(Console.ReadLine().TrimEnd().Split(' ').ToList());
            }

            List<int> result = Result.contacts(queries);

            Console.WriteLine(String.Join("\n", result));

            //textWriter.Flush();
            //textWriter.Close();
        }


        public static int SumOfLeafes(TreeNode root)
        {
            if (root.Left == null && root.Right == null) return root.Value;
            int sum = SumOfLeafes(root.Left) + SumOfLeafes(root.Right);
            return sum;
        }
        public class TreeNode
        {
            public int Value { get; set; }
            public TreeNode Left { get; set; }
            public TreeNode Right { get; set; }
        }

        public static int[] findNumbers(int[] nums)
        {
            int[] result = null;
            for (int i = 0; i < nums.Length; i++)
                if (nums[i] != i + 1)
                    result = new[] { nums[i], i + 1 };
            return result;
        }
        public static int CountElems(int[] arr)
        {
            return arr.Where(x => arr.Contains(x + 1)).Select(x => x).Count();
        }
        public static int CalcPoints(string[] ops)
        {
            var result = new List<int>();
            int index = 0;
            var charOpearions = new List<string> { "C", "D", "+" };
            ops.ToList().ForEach(x =>
            {
                if (charOpearions.Contains(x.Trim()))
                {
                    switch (x)
                    {
                        case "C":
                            if (result.Any())
                            {
                                result.RemoveAt(index - 1);
                                index--;
                            }
                            break;
                        case "D":
                            if (result.Any())
                            {
                                result.Add(result[index - 1] * 2);
                                index++;
                            }
                            break;
                        case "+":
                            if (result.Count >= 2)
                            {
                                result.Add(result[index - 1] + result[index - 2]);
                                index++;
                            }

                            break;
                    }
                }
                else { result.Add(int.Parse(x)); index++; }

            });
            return result.Sum();
        }
    }

    class Result
    {

        /*
         * Complete the 'contacts' function below.
         *
         * The function is expected to return an INTEGER_ARRAY.
         * The function accepts 2D_STRING_ARRAY queries as parameter.
         */

        public static List<int> contacts(List<List<string>> queries)
        {
            var result = new List<int>();
            var currentList = new List<string>();
            queries.ForEach(item =>
            {
                if (item[0].Equals("add"))
                    currentList.Add(item[1]);
                if (item[0].Equals("find"))
                    result.Add(currentList.Count(c => c.Contains(item[1])));
            });
            return result;
        }

        public static List<int> reverseArray(List<int> a)
        {
            var result = new List<int>();
            int count = a.Count;
            while (--count > 0) { result.Add(a[count]); }
            return result;
        }

        public static int hourglassSum(List<List<int>> arr)
        {
            int R = 5;
            int C = 5;
            int max_value = int.MinValue;
            for(int i = 0; i <= R - 2; i++)
            {
                for(int j = 0; j <= C - 2; j++)
                {
                    int sum = arr[i][j] + arr[i][j + 1] + arr[i][j + 2] +
                                          arr[i + 1][j + 1] +
                              arr[i+2][j] + arr[i+2][j + 1] + arr[i+2][j + 2];
                        max_value = Math.Max(sum, max_value);
                }
            }
            return max_value;
        }
    }
}
