using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Algorithms
{
    public class MyStack<T>
    {
        internal class StackNode<T>
        {
            public T data;
            public StackNode<T> next;
            public StackNode(T data)
            {
                this.data = data;
            }
        }

        private StackNode<T> _top;
        public T Pop()
        {
            if (_top == null) throw new NotImplementedException();
            T item = _top.data;
            _top = _top.next;
            return item;
        }
        public void Push(T item)
        {
            StackNode<T> t = new StackNode<T>(item);
            t.next = _top;
            _top = t;
        }
        public T Peek()
        {
            if (_top == null) throw new NotImplementedException();
            return _top.data;
        }
        public bool IsEmpty() { return _top == null; }
    }
    public class Stacks
    {

        /*
         * Complete the 'getMax' function below.
         *
         * The function is expected to return an INTEGER_ARRAY.
         * The function accepts STRING_ARRAY operations as parameter.
         */

        public static string isBracketsBalanced(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "NO";
            s = s.Trim();
            if (s.Length % 2 != 0) return "NO";

            int[] openBracketsArray = new int[] { '{', '(', '[' };
            int[] closedBracketsArray = new int[] { '}', ')', ']' };

            //if (!s.Any(x => openBracketsArray.Contains(x))) return "NO";
            bool isopenBracketsStackUsed = false;
            var openBracketsStack = new Stack<int>();

            for (int i = 0; i < s.Length; i++)
            {
                if (openBracketsArray.Contains(s[i])) { openBracketsStack.Push(s[i]); isopenBracketsStackUsed = true; }
                else if (closedBracketsArray.Contains(s[i]) && openBracketsStack.Count > 0)
                {
                    var topBracket = openBracketsStack.Pop();
                    if (s[i] == closedBracketsArray[0] && topBracket != openBracketsArray[0]) return "NO";
                    else if (s[i] == closedBracketsArray[1] && topBracket != openBracketsArray[1]) return "NO";
                    else if (s[i] == closedBracketsArray[2] && topBracket != openBracketsArray[2]) return "NO";

                }
            }

            return isopenBracketsStackUsed && openBracketsStack.Count == 0 ? "YES" : "NO";
        }
        public bool IsValid(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return true;
            Dictionary<char, char> closedCharsMap = new Dictionary<char, char> { { ')', '(' }, { '}', '{' }, { ']', '[' } };
            Stack<char> opendChars = new Stack<char>();
            foreach (char item in s)
            {
                if (closedCharsMap.ContainsKey(item))
                {
                    char openedChar = closedCharsMap[item];
                    if (opendChars.Count == 0) return false;
                    char topChar = opendChars.Pop();
                    if (openedChar != topChar) return false;
                }
                else { opendChars.Push(item); }
            }
            return opendChars.Count == 0;
        }
        public int EvalRPN(string[] tokens)
        {
            string[] opTokens = new string[] { "+", "-", "*", "/" };
            Stack<int> opStack = new Stack<int>();
            foreach (string item in tokens)
            {
                if (!opTokens.Contains(item)) opStack.Push(int.Parse(item));
                else
                {
                    opStack.Push(CalculateOp(opStack.Pop(), opStack.Pop(), item));
                }
            }
            return opStack.Pop();
        }

        private int CalculateOp(int v1, int v2, string op)
        {
            switch (op)
            {
                case "+": return v1 + v2;
                case "-": return v2 - v1;
                case "*": return v1 * v2;
                case "/": return v2 / v1;
                default: return 0;
            }
        }

        public static List<int> getMax(List<string> operations)
        {
            var result = new List<int>();
            var originalStack = new Stack<int>();
            var tempMaxValueStack = new Stack<int>();
            operations.ForEach(operation =>
            {
                var operationParts = operation.Split(' ').Select(int.Parse).ToArray();
                if (operationParts[0] == 1)
                {
                    var newValue = operationParts[1];
                    originalStack.Push(newValue);
                    if (newValue >= GetMaxValue(tempMaxValueStack))
                        tempMaxValueStack.Push(newValue);
                }
                else if (operationParts[0] == 2)
                {
                    var removedValue = originalStack.Pop();
                    if (removedValue == GetMaxValue(tempMaxValueStack))
                        tempMaxValueStack.Pop();
                }
                else if (operationParts[0] == 3) result.Add(GetMaxValue(tempMaxValueStack));
            });
            return result;
        }
        public static int equalStacks(List<int> h1, List<int> h2, List<int> h3)
        {
            int minValue = int.MinValue;
            int[] heights = new int[3] { 0, 0, 0 };
            List<Stack<int>> listOfStacks = new List<Stack<int>>();

            Stack<int> stack1 = new Stack<int>();
            for (int i = h1.Count - 1; i >= 0; i--)
            {
                stack1.Push(h1[i]);
                heights[0] += h1[i];
            }
            listOfStacks.Add(stack1);

            Stack<int> stack2 = new Stack<int>();
            for (int i = h2.Count - 1; i >= 0; i--)
            {
                stack2.Push(h2[i]);
                heights[1] += h2[i];
            }
            listOfStacks.Add(stack2);

            Stack<int> stack3 = new Stack<int>();
            for (int i = h3.Count - 1; i >= 0; i--)
            {
                stack3.Push(h3[i]);
                heights[2] += h3[i];
            }
            listOfStacks.Add(stack3);

            minValue = heights.Min();
            while (heights.Any(c => c > minValue))
            {
                for (int i = 0; i < heights.Length; i++)
                {
                    if (heights[i] > minValue)
                    {
                        heights[i] -= listOfStacks[i].Pop();
                    }
                }
                minValue = heights.Min();
            }

            return minValue;
        }
        public static int twoStacks(int maxSum, List<int> a, List<int> b)
        {
            int totalSum = 0;
            int stack1Count = 0;
            int stack2Count = 0;
            int numberOfValues;

            for (int i = 0; i < a.Count; i++)
            {
                if (totalSum + a[i] > maxSum) break;
                totalSum += a[i];
                stack1Count++;
            }
            numberOfValues = stack1Count;

            for (int i = 0; i < b.Count; i++)
            {
                totalSum += b[i];
                stack2Count++;
                while (totalSum > maxSum && stack1Count > 0)
                {
                    totalSum -= a[stack1Count - 1];
                    stack1Count--;
                }
                numberOfValues = totalSum <= maxSum ? Math.Max(stack1Count + stack2Count, numberOfValues) : numberOfValues;
            }
            return numberOfValues;
        }
        public static int twoStacks2(int maxSum, List<int> a, List<int> b)
        {
            int totalSum = 0;
            int stack1TotalSum = 0;
            int stack2TotalSum = 0;
            int numberOfValues = 0;

            Stack<int> stack1 = new Stack<int>();
            for (int i = a.Count - 1; i >= 0; i--)
            {
                stack1.Push(a[i]);
            }

            Stack<int> stack2 = new Stack<int>();
            for (int i = b.Count - 1; i >= 0; i--)
            {
                stack2.Push(b[i]);
            }

            while (true)
            {
                int nextSmallestValue = 0;
                if (stack1.Count == 0 && stack2.Count > 0)
                {
                    nextSmallestValue = stack2.Pop();
                    stack2TotalSum += nextSmallestValue;
                }
                else if (stack1.Count > 0 && stack2.Count == 0)
                {
                    nextSmallestValue = stack1.Pop();
                    stack1TotalSum += nextSmallestValue;
                }
                else if (stack1TotalSum <= stack2TotalSum && (stack1.Peek() + totalSum) <= maxSum)
                {
                    nextSmallestValue = stack1.Pop();
                    stack1TotalSum += nextSmallestValue;
                }
                else if (stack1TotalSum > stack2TotalSum && (stack2.Peek() + totalSum) <= maxSum)
                {
                    nextSmallestValue = stack2.Pop();
                    stack2TotalSum += nextSmallestValue;
                }
                //else if (stack1.Peek() <= stack2.Peek())
                //{
                //    nextSmallestValue = stack1.Pop();
                //    stack1TotalSum += nextSmallestValue;
                //}
                //else if (stack1.Peek() > stack2.Peek())
                //{
                //    nextSmallestValue = stack2.Pop();
                //    stack2TotalSum += nextSmallestValue;
                //}
                if (nextSmallestValue + totalSum > maxSum) break;

                totalSum += nextSmallestValue;
                numberOfValues++;
            }
            return numberOfValues;
        }
        public static List<int> waiter(List<int> number, int q)
        {
            var primeNumberList = GetPrimeNumbers(q);

            // Stack<int> A = new Stack<int>();
            Stack<int> B = new Stack<int>();

            List<int> Answer = new List<int>();
            List<int> temp = number;

            int currentIteration = 1;
            while (currentIteration <= q)
            {
                List<int> temp2 = new List<int>();
                for (int i = temp.Count - 1; i >= 0; i--)
                {
                    if (temp[i] % primeNumberList[currentIteration - 1] == 0)
                    {
                        B.Push(temp[i]);
                    }
                    else
                    {
                        temp2.Add(temp[i]);
                    }
                }
                while (B.Count > 0) { Answer.Add(B.Pop()); }
                temp = temp2;// new List<int>();

                //while (A.Count > 0) { temp.Add(A.Pop()); }

                currentIteration++;
            }

            if (temp.Any()) Answer.AddRange(temp.Reverse<int>());
            return Answer;
        }
        public static long largestRectangle(List<int> h)
        {
            int largestArea = 0;
            for (int i = 0; i < h.Count; i++)
            {
                int tempAreaSum = 0;

                int index = i - 1;
                for (int j = index; j >= 0; j--)
                {
                    if (h[j] >= h[i])
                        tempAreaSum += h[i];
                    else break;
                }

                index = i;
                for (int j = index; j < h.Count; j++)
                {
                    if (h[j] >= h[i])
                        tempAreaSum += h[i];
                    else break;
                }

                largestArea = Math.Max(largestArea, tempAreaSum);
            }

            return largestArea;
        }
        public static void textEditor(string editorValue, List<List<string>> operations)
        {
            var stackOperations = new Stack<string>();
            var text = editorValue;
            stackOperations.Push(text);
            var cachedValues = new Dictionary<int, char>();
            operations.ForEach(op =>
            {
                int operationType = Convert.ToInt32(op[0]);
                if (operationType == 1)
                {
                    var valueToAppend = op[1];
                    text = stackOperations.Peek() + valueToAppend;
                    stackOperations.Push(text);
                }
                else if (operationType == 2)
                {
                    var numberOfCharactersToDelete = Convert.ToInt32(op[1]);
                    text = stackOperations.Peek().Substring(0, stackOperations.Peek().Length - numberOfCharactersToDelete);
                    stackOperations.Push(text);
                    cachedValues.Clear();
                }
                else if (operationType == 3)
                {
                    var charIndexToPrint = Convert.ToInt32(op[1]);
                    if (cachedValues.ContainsKey(charIndexToPrint))
                        Console.WriteLine(cachedValues[charIndexToPrint]);
                    else
                    {
                        var charToPrint = text[charIndexToPrint - 1];
                        Console.WriteLine(charToPrint);
                        cachedValues[charIndexToPrint] = charToPrint;
                    }
                }
                else if (operationType == 4)
                {
                    stackOperations.Pop();
                    text = stackOperations.Peek();
                    cachedValues.Clear();
                }
            });
        }
        public static void textEditor2(string editorValue, List<List<string>> operations)
        {
            var stackOperations = new Stack<string>();
            var sb = new StringBuilder(editorValue);
            stackOperations.Push(sb.ToString());
            //var result = new List<string>();
            operations.ForEach(op =>
            {
                int operationType = Convert.ToInt32(op[0]);
                if (operationType == 1)
                {
                    var valueToAppend = op[1];
                    sb.Append(valueToAppend);
                    stackOperations.Push(sb.ToString());
                }
                else if (operationType == 2)
                {
                    var numberOfCharactersToDelete = Convert.ToInt32(op[1]);
                    // sb.Remove(0, sb.Length - numberOfCharactersToDelete);
                    sb.Remove(sb.Length - numberOfCharactersToDelete, numberOfCharactersToDelete);
                    stackOperations.Push(sb.ToString());
                }
                else if (operationType == 3)
                {
                    var charIndexToPrint = Convert.ToInt32(op[1]);
                    Console.WriteLine(sb[charIndexToPrint - 1]);
                    //result.Add(sb[charIndexToPrint - 1].ToString());
                }
                else if (operationType == 4)
                {
                    stackOperations.Pop();
                    sb = new StringBuilder(stackOperations.Peek());
                }
            });
        }
        private static int GetMaxValue(Stack<int> maxValuesStack)
        {
            return maxValuesStack.Count == 0 ? int.MinValue : maxValuesStack.Peek();
        }

        private static int[] GetPrimeNumbers(int numOfIterations)
        {
            int[] result = new int[numOfIterations];
            int currentCount = 0;
            for (int i = 2; i <= int.MaxValue && currentCount < numOfIterations; i++)
            {
                int counter = 0;
                for (int j = 2; j <= i / 2; j++)
                {
                    if (i % j == 0)
                    {
                        counter++;
                        break;
                    }
                }

                if (counter == 0 && i != 1)
                {
                    result[currentCount++] = i;
                }
            }

            return result;
        }
    }

    class MaxElementInStackSolution
    {
        public static void Main(string[] args)
        {
            TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

            int n = Convert.ToInt32(Console.ReadLine().Trim());

            List<string> ops = new List<string>();

            for (int i = 0; i < n; i++)
            {
                string opsItem = Console.ReadLine();
                ops.Add(opsItem);
            }

            List<int> res = Stacks.getMax(ops);

            textWriter.WriteLine(String.Join("\n", res));

            textWriter.Flush();
            textWriter.Close();
        }
    }

}
