using Encryption.CaesarCipher;
using Enterprise.Storage;
using Enterprise.Try;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;

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

            //TestContracts();

            //bool result = new NetworkDriveUtility().MountNetworkDrive();
            //SFTPClient client = new SFTPClient();
            //client.Upload();
            //client.Download();
            // client.Remove();
            //var result = ConvertHexStringToByteArray("42 4D");

            //HandleXmlSchema("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<TaskData xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schema.ultimus.com/CompletionTimeProcess/1/447995\">\r\n  <Global>\r\n    <Users xmlns=\"http://schema.ultimus.com/CompletionTimeProcess/1/Types\">1,2,3,4,5,6</Users>\r\n    <TotalQuantity xmlns=\"http://schema.ultimus.com/CompletionTimeProcess/1/Types\">200</TotalQuantity>\r\n  </Global>\r\n  <SYS_INCIDENTMEMO>\r\n    <Data xmlns=\"http://schema.ultimus.com/processglobals\">Ult</Data>\r\n    <EntryDate xmlns=\"http://schema.ultimus.com/processglobals\">2023-04-11T14:30:16.32</EntryDate>\r\n    <User xmlns=\"http://schema.ultimus.com/processglobals\">DESKTOP-MMO5URK/AhmedEisa</User>\r\n    <StepName xmlns=\"http://schema.ultimus.com/processglobals\">Begin</StepName>\r\n  </SYS_INCIDENTMEMO>\r\n  <SYS_INCIDENTMEMO>\r\n    <Data xmlns=\"http://schema.ultimus.com/processglobals\">TestMemo</Data>\r\n    <EntryDate xmlns=\"http://schema.ultimus.com/processglobals\">2023-04-11T14:30:35.565</EntryDate>\r\n    <User xmlns=\"http://schema.ultimus.com/processglobals\">DESKTOP-MMO5URK/AhmedEisa</User>\r\n    <StepName xmlns=\"http://schema.ultimus.com/processglobals\">UpdateQuantities</StepName>\r\n  </SYS_INCIDENTMEMO>\r\n  <SYS_PROCESSATTACHMENTS />\r\n</TaskData>");

            //Guid guid = Guid.NewGuid();
            //byte[] guidBytes = guid.ToByteArray();

            //// Convert the GUID to a 128-bit integer
            //ulong high = BitConverter.ToUInt64(guidBytes, 8);
            //ulong low = BitConverter.ToUInt64(guidBytes, 0);
            //ulong guidInt = (high << 64) + low;

            // Hash the 128-bit integer to a 64-bit integer
            //HashAlgorithm hash = SHA256.Create();
            //byte[] hashBytes = hash.ComputeHash(BitConverter.GetBytes(guidInt));
            //long guidNumber = BitConverter.ToInt64(hashBytes, 0);

            //Console.WriteLine(guidNumber);

            //Color normal = Color.Blue | Color.Green;
            //Color blueColor = Color.Blue;
            //Console.WriteLine(blueColor & Color.Red);

            List<CompletionStateType> normal = new List<CompletionStateType> { CompletionStateType.AllTasks, CompletionStateType.AtLeastOneTask };
            Console.WriteLine(normal.Contains(CompletionStateType.AtLeastOneTask));


            Console.WriteLine(DateTime.UtcNow);
            Console.ReadLine();
        }
        public static string timeConversion(string s)
        {
            var timeParts = s.Split(':');
            int hourValue = int.Parse(timeParts[0]);
            int newValue = hourValue;
            if (s.Contains("PM") && hourValue < 12)
                newValue = hourValue + 12;
            if (s.Contains("AM") && hourValue >= 12)
                newValue = 0;
            return string.Join(':', string.Format("{0:00}", newValue), timeParts[1], timeParts[2].Replace("PM", string.Empty).Replace("AM", string.Empty));
        }

        static void CalcCominationsOfLettersFromDigits()
        {
            Dictionary<int, List<char>> digitMapping = new Dictionary<int, List<char>>();
            digitMapping.Add(2, new List<char>() { 'a', 'b', 'c' });
            digitMapping.Add(3, new List<char>() { 'd', 'e', 'f' });
            digitMapping.Add(4, new List<char>() { 'g', 'h', 'i' });

            int[] digits = new int[] { 2, 3, 4 };
            List<string> combinations = GenerateCombinations(digits, digitMapping);
            foreach (string combination in combinations)
            {
                Console.WriteLine(combination);
            }
        }
        static List<string> GenerateCombinations(int[] digits, Dictionary<int, List<char>> digitMapping)
        {
            List<string> combinations = new List<string>();
            GenerateCombinationsHelper(digits, 0, "", combinations, digitMapping);
            return combinations;
        }
        static void GenerateCombinationsHelper(int[] digits, int index, string currentCombination, List<string> combinations, Dictionary<int, List<char>> digitMapping)
        {
            if (index == digits.Length)
            {
                combinations.Add(currentCombination);
                return;
            }

            int digit = digits[index];
            List<char> chars = digitMapping[digit];

            foreach (char c in chars)
            {
                GenerateCombinationsHelper(digits, index + 1, currentCombination + c, combinations, digitMapping);
            }
        }

        public static int MaxCost(List<int> cost, List<string> labels, int dailyCount)
        {
            int maxCost = 0;
            int NumberOfCurrentLegalDevices = 0;
            int currentCost = 0;
            for (int i = 0; i < labels.Count; i++)
            {
                if (NumberOfCurrentLegalDevices == dailyCount)
                {
                    NumberOfCurrentLegalDevices = 0;
                    currentCost = 0;
                    currentCost += cost[i + 1];
                    NumberOfCurrentLegalDevices += labels[i] == "legal" ? 1 : 0;
                    maxCost = currentCost > maxCost ? currentCost : maxCost;
                }
                else
                {
                    currentCost += cost[i + 1];
                    NumberOfCurrentLegalDevices += labels[i] == "legal" ? 1 : 0;
                }
            }
            if (NumberOfCurrentLegalDevices == dailyCount)
            {
                maxCost = currentCost > maxCost ? currentCost : maxCost;
            }
            return maxCost;
        }
        enum CompletionStateType
        {
            /// <summary>
            /// AllTasks
            /// </summary>
            AllTasks = 1,

            /// <summary>
            /// AtLeastOneTask
            /// </summary>
            AtLeastOneTask = 2,

            /// <summary>
            /// ManualCompletionTimeExpiry
            /// </summary>
            ManualCompletionTimeExpiry = 4,

            /// <summary>
            /// AutoCompletionTimeExpiry
            /// </summary>
            AutoCompletionTimeExpiry = 5,
        }
        enum Color
        {
            Red = 1,
            Blue = 2,
            Green = 4
        }
        private static void HandleXmlSchema(string taskXml)
        {
            XDocument doc = XDocument.Parse(taskXml);
            string rootNamespace = string.Concat("{", doc.Root.Name.NamespaceName, "}");

            foreach (XElement el in doc.Elements($"{rootNamespace}TaskData").Elements($"{rootNamespace}Global").Elements())
            {
                Console.WriteLine("Name: " + el.Name.LocalName);
                Console.WriteLine("Value: " + el.Value);
            }

        }

        private static void HandleXmlSchemaEx(string taskXml)
        {
            XDocument doc = XDocument.Parse(taskXml);
            string rootNamespace = string.Concat("{", doc.Root.Name.NamespaceName, "}");

            foreach (XElement el in doc.Elements($"{rootNamespace}TaskData").Elements($"{rootNamespace}Global").Elements())
            {
                Console.WriteLine("Name: " + el.Name.LocalName);
                Console.WriteLine("Value: " + el.Value);
            }

        }
        public static byte[] StrToByteArray(string str)
        {
            Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
            for (int i = 0; i <= 255; i++)
                hexindex.Add(i.ToString("X2"), (byte)i);

            List<byte> hexres = new List<byte>();
            for (int i = 0; i < str.Length; i += 2)
                hexres.Add(hexindex[str.Substring(i, 2)]);

            return hexres.ToArray();
        }

        public static byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            byte[] data = new byte[hexString.Length / 2];
            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
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
            for (int i = 0; i <= R - 2; i++)
            {
                for (int j = 0; j <= C - 2; j++)
                {
                    int sum = arr[i][j] + arr[i][j + 1] + arr[i][j + 2] +
                                          arr[i + 1][j + 1] +
                              arr[i + 2][j] + arr[i + 2][j + 1] + arr[i + 2][j + 2];
                    max_value = Math.Max(sum, max_value);
                }
            }
            return max_value;
        }
    }


    // you can also use other imports, for example:
    // using System.Collections.Generic;

    // you can write to stdout for debugging purposes, e.g.
    // Console.WriteLine("this is a debug message");

    public class SolutionFairIndex
    {
        public int solution(int[] A, int[] B)
        {
            int arraysLength = A.Length;
            double[] aggregatedSumA = new double[arraysLength];
            double[] aggregatedSumB = new double[arraysLength];

            AggregateArraysSum(A, B, aggregatedSumA, aggregatedSumB);

            //aggregatedSumA[0] = A[0];
            //aggregatedSumB[0] = B[0];
            //for (int i = 1; i < arraysLength - 1; i++)
            //{
            //    aggregatedSumA[i] = A[i] + aggregatedSumA[i - 1];
            //    aggregatedSumB[i] = B[i] + aggregatedSumB[i - 1];
            //}

            int totalFairIndexCount = 0;
            for (int i = 1; i < arraysLength - 2; i++)
                if (IsFairIndex(i, aggregatedSumA, aggregatedSumB, arraysLength))
                    totalFairIndexCount++;

            return totalFairIndexCount;
        }
        private void AggregateArraysSum(int[] A, int[] B, double[] aggregatedSumA, double[] aggregatedSumB)
        {
            int arraysLength = A.Length;
            aggregatedSumA[0] = A[0];
            aggregatedSumB[0] = B[0];
            for (int i = 1; i < arraysLength - 1; i++)
            {
                aggregatedSumA[i] = A[i] + aggregatedSumA[i - 1];
                aggregatedSumB[i] = B[i] + aggregatedSumB[i - 1];
            }
        }
        private bool IsFairIndex(int k, double[] aggregatedSumA, double[] aggregatedSumB, int arraysLength)
        {

            if (aggregatedSumA[k - 1] == aggregatedSumB[k - 1] &&
                (aggregatedSumA[arraysLength - 1] - aggregatedSumA[k - 1]) == (aggregatedSumB[arraysLength - 1] - aggregatedSumB[k - 1]))
                return true;
            return false;
        }
    }


}
