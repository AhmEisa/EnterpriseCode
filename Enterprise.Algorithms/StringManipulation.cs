using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Algorithms
{
    public class StringManipulation
    {
        public static string Test(string input)
        {
            int shift = 5;
            string output = "";
            if (input != null)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] < 65 || input[i] > 90)
                    {
                        throw new Exception("Only A-Z supported.");
                    }
                    int shifted = input[i] + shift;
                    if (shifted > 90)
                    {
                        shifted = 65 + shifted - 91;
                    }

                    output = output + (char)shifted;
                }
            }

            return output;
        }
        public static string ShiftStringCharacters(string input, int numberOfShifts, int startCharValue, int endCharValue)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            if (input.Any(c => c < startCharValue || c > endCharValue))
                throw new Exception($"Only {(char)startCharValue}-{(char)endCharValue}  supported.");

            StringBuilder output = new StringBuilder();
            Dictionary<char, char> cachedChars = new Dictionary<char, char>();

            for (int i = 0; i < input.Length; i++)
            {
                if (cachedChars.TryGetValue(input[i], out char cachedChar))
                {
                    output.Append(cachedChar);
                    continue;
                }

                int shifted = input[i] + numberOfShifts;
                if (shifted > endCharValue)
                {
                    shifted = startCharValue + shifted - (endCharValue + 1);
                }

                output.Append(cachedChars[input[i]] = (char)shifted);
            }

            return output.ToString();
        }


    }
}

namespace Encryption.CaesarCipher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int shift = 5;
            string output = "";
            Console.Write("Input: ");
            string input = Console.ReadLine();

            output = ShiftStringCharacters(input, shift, 65, 90);

            Console.WriteLine("Output: " + output);
        }
        public static string ShiftStringCharacters(string input, int numberOfShifts, int startCharValue, int endCharValue)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            if (input.Any(c => c < startCharValue || c > endCharValue))
                throw new Exception($"Only {(char)startCharValue}-{(char)endCharValue}  supported.");

            StringBuilder output = new StringBuilder();
            Dictionary<char, char> cachedChars = new Dictionary<char, char>();

            for (int i = 0; i < input.Length; i++)
            {
                if (cachedChars.TryGetValue(input[i], out char cachedChar))
                {
                    output.Append(cachedChar);
                    continue;
                }

                int shifted = input[i] + numberOfShifts;
                if (shifted > endCharValue)
                {
                    shifted = startCharValue + shifted - (endCharValue + 1);
                }

                output.Append(cachedChars[input[i]] = (char)shifted);
            }
           
            return output.ToString();
        }
    }
}
