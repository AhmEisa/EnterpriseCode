using System;
using System.Collections.Generic;
using System.IO;

namespace Enterprise.Algorithms
{
    public class MatchingStringsResult
    {

        /*
         * Complete the 'matchingStrings' function below.
         *
         * The function is expected to return an INTEGER_ARRAY.
         * The function accepts following parameters:
         *  1. STRING_ARRAY strings
         *  2. STRING_ARRAY queries
         */

        public static List<int> matchingStrings(List<string> strings, List<string> queries)
        {
            var result = new List<int>();
            var dictionary = new Dictionary<string, int>();
            strings.ForEach(item =>
            {
                if (dictionary.ContainsKey(item))
                {
                    dictionary[item] = dictionary[item] + 1;
                }
                else dictionary[item] = 1;
            });

            queries.ForEach(item =>
            {
                if (dictionary.ContainsKey(item)) result.Add(dictionary[item]);
                else result.Add(0);
            });

            return result;
        }

    }
    class MatchingStringsSolution
    {
        public static void Main(string[] args)
        {
            TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

            int stringsCount = Convert.ToInt32(Console.ReadLine().Trim());

            List<string> strings = new List<string>();

            for (int i = 0; i < stringsCount; i++)
            {
                string stringsItem = Console.ReadLine();
                strings.Add(stringsItem);
            }

            int queriesCount = Convert.ToInt32(Console.ReadLine().Trim());

            List<string> queries = new List<string>();

            for (int i = 0; i < queriesCount; i++)
            {
                string queriesItem = Console.ReadLine();
                queries.Add(queriesItem);
            }

            List<int> res = MatchingStringsResult.matchingStrings(strings, queries);

            textWriter.WriteLine(String.Join("\n", res));

            textWriter.Flush();
            textWriter.Close();
        }
    }

}
