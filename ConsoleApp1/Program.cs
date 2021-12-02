using Enterprise.Try;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var result = SumOfLeafes(root);
            //Print counter
           // Console.WriteLine("Counter is: {0}", result);

           // var textExtracted = ExtractTextFromFiles.ExtractText(@"D:\Work Files\Techno-Ways\Geidea Gateway_ DirectAPI Guide v1.2.pdf");
           // Console.WriteLine("Text in pdf : {0}", textExtracted);

            //DocumentStore documentStore = new DocumentStore(2);
            //documentStore.AddDocument("item");
            //Console.WriteLine(documentStore);

            ChainLink left = new ChainLink();
            ChainLink middle = new ChainLink();
            ChainLink right = new ChainLink();
            left.Append(middle);
            middle.Append(right);
            Console.WriteLine(left.LongerSide());

            CodePerformance.Test();
           

            Console.ReadLine();
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
}
