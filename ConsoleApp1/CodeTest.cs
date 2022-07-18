using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Encryption.CaesarCipher
{
    public class Program3
    {
        public void MainMehtod(string[] args)
        {
            StringBuilder output = new StringBuilder();

            Console.Write("Input: ");
            string input = Console.ReadLine();

            if (input != null)
            {
                int numberOfshifts = 5;
                var inputCharacters = input.ToCharArray();
                inputCharacters.Select(r => new CapitalCharacter(r).ShiftedBy(numberOfshifts)).ToList().ForEach(r => output.Append(r.Value));
            }

            Console.WriteLine("Output: " + output.ToString());
        }
    }
    public class CapitalCharacter
    {
        static int A = 65;
        static int Z = 90;
        static int BaseRound = 91;
        public char Value { get; }
        public CapitalCharacter(char value)
        {
            if (value < A || value > Z)
            {
                throw new Exception("Only A-Z supported.");
            }
            Value = value;
        }

        public CapitalCharacter ShiftedBy(int numberOfShifts)
        {
            int shiftedCharValue = this.Value + numberOfShifts;
            if (shiftedCharValue > Z)
            {
                shiftedCharValue = A + shiftedCharValue - BaseRound;
            }
            return new CapitalCharacter((char)shiftedCharValue);
        }
    }

    class Vehicle
    {
        public int NumberOfWheels { get; set; }

        public Vehicle(int numberOfWheels)
        {
            NumberOfWheels = numberOfWheels;
            PrintOutDescription();
        }


        protected virtual void PrintOutDescription()
        {
            Console.WriteLine($"The vehicle has {NumberOfWheels} wheels");
        }
    }


    class Bicycle : Vehicle
    {
        public Bicycle() : base(2)
        {
        }

        protected override void PrintOutDescription()
        {
            Console.WriteLine($"The bicycle has {NumberOfWheels} wheels");
        }
    }

    public class PlusMinsNumberDigitsToBeZero
    {
        public static void MainMethod()
        {
            var bicycle = new Bicycle();

            Console.WriteLine(PlusMinus(int.Parse(Console.ReadLine())));
        }

        //35132 - 26712
        public static string PlusMinus(int num)
        {
            List<int> digits = new List<int>();

            while (num > 0)
            {
                digits.Add(num % 10);
                num = num / 10;
            }
            digits.Reverse();

            if (digits.Count < 2)
                return "not possible";

            var firstDigit = digits[0];
            digits.RemoveAt(0);

            return plusMinusRec(digits, firstDigit);
        }
        public static string plusMinusRec(List<int> digits, int sum)
        {
            if (digits.Count == 1)
            {
                if (sum + digits[0] == 0)
                {
                    return "+";
                }
                else if (sum - digits[0] == 0)
                {
                    return "-";
                }
                else
                {
                    return "not possible";
                }
            }

            var updatedDigits = digits.ToList();

            var firstDigit = updatedDigits[0];
            updatedDigits.RemoveAt(0);
            string s2 = plusMinusRec(updatedDigits, sum - firstDigit);
            if (s2 != "not possible")
            {
                return "-" + s2;
            }

            string s1 = plusMinusRec(updatedDigits, sum + firstDigit);
            if (s1 != "not possible")
            {
                return "+" + s1;
            }

            return "not possible";
        }
        public static (string result, int sum) Plus(string result, List<int> arr, int sum, int newNum)
        {
            for (int i = 0; i < arr.Count; i++)
                Plus(result + "+", arr, arr[i], arr[i + 1]);
            return (result + "+", sum + newNum);
        }
        public static (string result, int sum) Minus(string result, int sum, int newNum)
        {
            return (result + "-", sum - newNum);
        }
    }
    public class SubtractDigitsFromString
    {
        public static string Subtract(string input)
        {
            // Split on one or more non-digit characters.
            string[] numbers = Regex.Split(input, @"\D+");
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int i = int.Parse(value);
                    Console.WriteLine("Number: {0}", i);
                }
            }
            return string.Join(',', numbers);
        }
    }
}