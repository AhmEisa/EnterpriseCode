using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Algorithms
{
    public static class Strings
    {
        public static string passwordCracker(List<string> passwords, string loginAttempt)
        {
            string matchedString = passwords.FirstOrDefault(x => x == loginAttempt);
            if (!string.IsNullOrWhiteSpace(matchedString)) { return matchedString; }

            List<string> result = new List<string>();
            string remainedLoginAttempt = loginAttempt;
            int index = 0;
            int end = passwords.Count;
            while (index < end)
            {
                if (string.IsNullOrEmpty(remainedLoginAttempt)) { return string.Join(" ", result); }
                if (passwords[index].Length > remainedLoginAttempt.Length) { index++; continue; }


                string currentElm = passwords[index];
                string matchedLoginAttempt = remainedLoginAttempt.Substring(0, currentElm.Length);
                if (currentElm == matchedLoginAttempt)
                {
                    result.Add(currentElm);
                    remainedLoginAttempt = remainedLoginAttempt.Substring(currentElm.Length);
                    index = 0;
                }
                else { index++; }
            }
            return "WRONG PASSWORD";
        }
        public static string timeConversion(string s)
        {
            bool PM = s.Contains("PM");
            bool AM = s.Contains("AM");
            string[] timeParts = (PM ? s.Replace("PM", "") : s.Replace("AM", "")).Split(":");
            int hours = int.Parse(timeParts[0]);
            if (PM && hours != 12) hours += 12;
            if (AM && hours == 12) hours = 0;
            return string.Format("{0:00}:{1}:{2}", hours, timeParts[1], timeParts[2]);
        }
        //public static string AddBinary(string a, string b)
        //{
        //    if (a.Length < b.Length) return AddBinary(b, a);
        //    int carry = 0;
        //    int n = a.Length - 1;
        //    int m = b.Length - 1;
        //    var resultBuilder = new StringBuilder();
        //    for (int i = n; i >= 0; i--)
        //    {
        //        if (a[i] == '1') carry++;
        //        if (m >= 0 && b[m--] == '1') carry++;
        //        if (carry % 2 == 1) resultBuilder.Append('1');
        //        else resultBuilder.Append('0');
        //        carry /= 2;
        //    } 
        //    if (carry == 1) resultBuilder.Append('1'); 
        //   .string result = resultBuilder.ToString();
        //    return result.Reverse().ToString();
        //}
        /*
         Given two strings needle and haystack, return the index of the first occurrence of needle in haystack, or -1 if needle is not part of haystack.

 

            Example 1:

            Input: haystack = "sadbutsad", needle = "sad"
            Output: 0
            Explanation: "sad" occurs at index 0 and 6.
            The first occurrence is at index 0, so we return 0.
         */
        public static int StrStr(string haystack, string needle)
        {
            int m = needle.Length;
            int n = haystack.Length;
            for (int windowStart = 0; windowStart <= n - m; windowStart++)
            {
                for (int i = 0; i < m; i++)
                {
                    if (needle[i] != haystack[windowStart + i])
                    {
                        break;
                    }
                    if (i == m - 1)
                    {
                        return windowStart;
                    }
                }
            }

            return -1;
        }
        public static string LongestCommonPrefix(string[] strs)
        {
            if (strs.Length == 0) return "";
            string prefix = strs[0];
            for (int i = 1; i < strs.Length; i++)
                while (strs[i].IndexOf(prefix) != 0)
                {
                    prefix = prefix.Substring(0, prefix.Length - 1);
                    if (string.IsNullOrEmpty(prefix)) return "";
                }
            return prefix;
        }
        public static void ReverseString(char[] s)
        {
            int left = 0,
                right = s.Length - 1;

            while (left < right)
            {
                char tmp = s[left];
                s[left++] = s[right];
                s[right--] = tmp;
            }

            helper(s, 0, s.Length - 1);
        }
        private static void helper(char[] s, int left, int right)
        {
            if (left >= right) return;
            char tmp = s[left];
            s[left++] = s[right];
            s[right--] = tmp;
            helper(s, left, right);
        }

        public static string RemoveDuplicates(String s)
        {
            StringBuilder stack = new StringBuilder();
            foreach (char c in s.ToArray())
            {
                if (stack.Length > 0 && stack[stack.Length - 1] == c)
                {
                    stack.Remove(stack.Length - 1, 1);
                }
                else
                {
                    stack.Append(c);
                }
            }

            return stack.ToString();
        }
        public static string ConvertToBase7(int num)
        {

            if (num == 0) return "0";
            int numToConvert = num > 0 ? num : num * -1;
            int remained = numToConvert;
            StringBuilder result = new StringBuilder(); // can be replaced with a stack to reverse the result string
            while (remained > 0)
            {
                result.Append(remained % 7);
                remained /= 7;
            }
            StringBuilder revResult = new StringBuilder();
            if (num < 0) revResult.Append("-");
            for (int i = result.Length - 1; i >= 0; i--)
            {
                revResult.Append(result[i]);
            }
            return revResult.ToString();
        }
        //public static string ModifyString()
        //{
        //    string s1 = "Hello World";
        //    var s2 = s1.ToCharArray();
        //    s1[5] = ',';
        //    Console.WriteLine(s1);
        //    return s1;
        //}
    }
}
