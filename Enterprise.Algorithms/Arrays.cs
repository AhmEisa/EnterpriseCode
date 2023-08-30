using System;
using System.Collections.Generic;
using System.Linq;

namespace Enterprise.Algorithms
{
    public static class Arrays
    {
        public static void plusMinus(List<int> arr)
        {
            int posNum = arr.Count(x => x > 0);
            int negNum = arr.Count(x => x < 0);
            int zeroNum = arr.Count(x => x == 0);
            int toalCount = arr.Count;
            Console.WriteLine(((double)posNum / toalCount).ToString("F6"));
            Console.WriteLine(((double)negNum / toalCount).ToString("F6"));
            Console.WriteLine(((double)zeroNum / toalCount).ToString("F6"));
        }
        public static void miniMaxSum(List<int> arr)
        {
            var sortedArr = arr.OrderBy(x => x);
            double min = sortedArr.Take(4).Select(r => (double)r).Sum();
            double max = sortedArr.TakeLast(4).Select(r=>(double)r).Sum();
            Console.WriteLine("{0} {1}", min, max);
        }
    }
}
