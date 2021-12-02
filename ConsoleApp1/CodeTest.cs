using System;
using System.Linq;
using System.Text;

namespace Encryption.CaesarCipher
{
    public class Program2
    {
        public static void Main(string[] args)
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
}