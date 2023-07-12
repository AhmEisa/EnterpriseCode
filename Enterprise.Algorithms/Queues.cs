using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Algorithms
{
    public class MyQueue<T>
    {
        internal class QueueNode<T>
        {
            public T data;
            public QueueNode<T> next;
            public QueueNode(T data)
            {
                this.data = data;
            }
        }

        private QueueNode<T> _top;
        private QueueNode<T> _bottom;
        public T Dequeue()
        {
            if (_top == null) throw new NotImplementedException();
            T item = _top.data;
            _top = _top.next;
            return item;
        }
        public void Enqueue(T item)
        {
            QueueNode<T> oldBottom = _bottom;
            _bottom = new QueueNode<T>(item);
            if (IsEmpty()) _top = _bottom;
            else oldBottom.next = _bottom;
        }
        public T Peek()
        {
            if (_top == null) throw new NotImplementedException();
            return _top.data;
        }
        public bool IsEmpty() { return _top == null; }
    }
    public class Queues
    {
        public static void QueueUsingTwoStacks(List<List<string>> operations)
        {
            Stack<string> A = new Stack<string>();
            Stack<string> B = new Stack<string>();

            operations.ForEach(op =>
            {
                int operationType = Convert.ToInt32(op[0]);
                if (operationType == 1)
                {
                    A.Push(op[1]);
                }
                else if (operationType == 2)
                {
                    if (B.Count <= 0)
                        while (A.Count > 0)
                        {
                            B.Push(A.Pop());
                        }
                    B.Pop();
                }
                else if (operationType == 3)
                {
                    if (B.Count <= 0)
                        while (A.Count > 0)
                        {
                            B.Push(A.Pop());
                        }
                    Console.WriteLine(B.Peek());
                }
            });
        }
        public static int minimumMoves(List<string> grid, int startX, int startY, int goalX, int goalY)
        {
            return 3;
        }
        public static int downToZero(int n)
        {
            int moves = 0;
            List<int> list = new List<int>();
            while (n > 0)
            {
                n = GetNextNumber(n);
                list.Add(n);
                moves++;
            }

            return moves;
        }
        private static int GetNextNumber(int n)
        {
            int nextDivisbleNumber = GetNextDivisbleNumber(n);
            if (nextDivisbleNumber == 0) return n - 1;
            return Math.Min(nextDivisbleNumber, n - 1);
        }
        private static int GetNextDivisbleNumber(int n)
        {
            int result = 0;

            for (int i = (int)Math.Sqrt(n); i >= 2; i--)
            {
                if (n % i == 0) { result = Math.Max(i, n / i); break; }
            }
            return result;
        }

        static void MainMethod(String[] args)
        {
            // TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

            int Q = Convert.ToInt32(Console.ReadLine());
            List<List<string>> operations = new List<List<string>>();
            for (int i = 0; i < Q; i++)
            {
                operations.Add(Console.ReadLine().TrimEnd().Split(' ').ToList());
            }

            QueueUsingTwoStacks(operations);

            // textWriter.WriteLine();

            // textWriter.Flush();
            // textWriter.Close();
        }
    }

    /*
     private static int minSteps(char[][] grid, XY curr, XY end, Set<XY> visited) {
        Queue<XY> queue = new LinkedList<XY>();
        queue.add(curr);
        visited.add(curr);

        Queue<Integer> step = new LinkedList<Integer>();
        step.add(0);

        while (!queue.isEmpty()) {
            XY nextXY = queue.remove();
            int hop = step.remove();

            if (nextXY.equals(end)) return hop;

            List<XY> next = getNext(grid, nextXY, visited);
            visited.addAll(next);
            queue.addAll(next);

            for (int i = 0; i < next.size(); i++) {
                step.add(hop + 1);
            }
        }

        return Integer.MAX_VALUE;
    }

    private static List<XY> getNext(char[][] grid, XY curr, Set<XY> visited) {
        List<XY> next = new ArrayList<XY>();

        if (curr.dir == null || curr.dir == Dir.Y) {
            int x = curr.x;
            for (int i = x - 1; i >= 0; i--) {
                if (grid[i][curr.y] == 'X') break;

                final XY xy = new XY(i, curr.y, Dir.X);
                if (!visited.contains(xy)) next.add(xy);
            }

            for (int i = x + 1; i < grid.length; i++) {
                if (grid[i][curr.y] == 'X') break;

                final XY xy = new XY(i, curr.y, Dir.X);
                if (!visited.contains(xy)) next.add(xy);
            }
        }

        if (curr.dir == null || curr.dir == Dir.X) {
            int y = curr.y;

            for (int i = y - 1; i >= 0; i--) {
                if (grid[curr.x][i] == 'X') break;

                final XY xy = new XY(curr.x, i, Dir.Y);
                if (!visited.contains(xy)) next.add(xy);
            }

            for (int i = y + 1; i < grid.length; i++) {
                if (grid[curr.x][i] == 'X') break;

                final XY xy = new XY(curr.x, i, Dir.Y);
                if (!visited.contains(xy)) next.add(xy);
            }
        }

        return next;
    }
     */
    public class XY
    {
        public int x, y;
        public Dir? dir;

        public XY(int x, int y)
        {
            this.x = x;
            this.y = y;
            dir = null;
        }

        public XY(int x, int y, Dir dir)
        {
            this.x = x;
            this.y = y;
            this.dir = dir;
        }

        public bool equals(Object o)
        {
            if (this == o) return true;
            XY xy = (XY)o;
            return x == xy.x && y == xy.y;
        }

        public int hashCode()
        {
            return 31 * x + y;
        }

        public string toString()
        {
            return "XY{" +
                    "x=" + x +
                    ", y=" + y +
                    ", dir=" + dir +
                    '}';
        }
    }

    public enum Dir
    {
        X, Y
    }
}
