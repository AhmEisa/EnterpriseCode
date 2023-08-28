using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
