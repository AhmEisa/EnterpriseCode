using System.Collections.Generic;

namespace Enterprise.Algorithms
{
    public static class Numbers
    {
        public static int Fib(int n)
        {
            return FibWithMemoization(n, new Dictionary<int, int>());
        }
        private static int FibWithMemoization(int n, Dictionary<int, int> pairs)
        {
            if (pairs.ContainsKey(n)) return pairs[n];

            int result;
            if (n < 2) { result = n; }
            else { result = FibWithMemoization(n - 1, pairs) + FibWithMemoization(n - 2, pairs); }
            pairs.Add(n, result);
            return result;
        }
        public static int ClimbStairs(int n)
        {
            return ClimbStairsWithMemoization(0, n, new Dictionary<int, int>());
        }
        private static int ClimbStairsWithMemoization(int s, int n, Dictionary<int, int> memos)
        {
            if (memos.ContainsKey(s)) return memos[s];

            int result;
            if (s > n) result = 0;
            else if (s == n) result = 1;
            else result = ClimbStairsWithMemoization(s + 1, n, memos) + ClimbStairsWithMemoization(s + 2, n, memos);
            memos.Add(s, result);
            return result;
        }
        public static uint NormalSum(uint x, uint y)
        {
            return x + y;
        }
        public static int NormalSum(int x, int y)
        {
            return x + y;
        }

    }
}
